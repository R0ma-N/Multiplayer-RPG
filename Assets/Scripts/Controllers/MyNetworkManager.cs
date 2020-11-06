﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    public Action<string> loginResponse;
    public Action<string> registerResponse;

    public bool ServerMode;
    private UserDataRepository _repository;
    void Start()
    {
        if (ServerMode)
        {
            StartServer();
            _repository = UserDataRepository.Instance;
            NetworkServer.UnregisterHandler(MsgType.Connect);
            NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnectCustom);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnServerLogin);
            NetworkServer.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnServerRegister);
        }

    }
    void OnServerLogin(NetworkMessage netMsg)
    {
        StartCoroutine(LoginUser(netMsg));
    }

    void OnServerRegister(NetworkMessage netMsg)
    {
        StartCoroutine(RegisterUser(netMsg));
    }

    void OnClientLogin(NetworkMessage netMsg)
    {
        loginResponse.Invoke(netMsg.reader.ReadString());
    }

    void OnClientRegister(NetworkMessage netMsg)
    {
        registerResponse.Invoke(netMsg.reader.ReadString());
    }

    void OnServerConnectCustom(NetworkMessage netMsg)
    {
        if (LogFilter.logDebug)
        {
            Debug.Log("NetworkManager:OnServerConnectCustom");
        }
        netMsg.conn.SetMaxDelay(maxDelay);
        OnServerConnect(netMsg.conn);
    }

    public void Login(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendLogin(login, pass));
    }

    public void Register(string login, string pass)
    {
        ClientConnect();
        StartCoroutine(SendRegister(login, pass));
    }
    IEnumerator SendLogin(string login, string pass)
    {
        while (!client.isConnected)
        {
            yield return null;
        }
        Debug.Log("client login");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Login, new UserMessage(login, pass));
    }

    IEnumerator SendRegister(string login, string pass)
    {
        while (!client.isConnected)
        {
            yield return null;
        }
        Debug.Log("client register");
        client.connection.Send(MsgType.Highest + (short)NetMsgType.Register, new UserMessage(login, pass));
    }
    IEnumerator LoginUser(NetworkMessage netMsg)
    {
        UserAccount account = new UserAccount(netMsg.conn);
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        IEnumerator e = account.LoginUser(msg.login, msg.pass);

        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        if (response == "Success")
        {
            Debug.Log("server login success");
            netMsg.conn.Send(MsgType.Scene, new StringMessage(SceneManager.GetActiveScene().name));
        }
        else
        {
            Debug.Log("server login fail");
            netMsg.conn.Send(MsgType.Highest + (short)NetMsgType.Login, new StringMessage(response));
        }
    }

    IEnumerator RegisterUser(NetworkMessage netMsg)
    {
        UserMessage msg = netMsg.ReadMessage<UserMessage>();
        //IEnumerator e = DCF.RegisterUser(msg.login, msg.pass, "");
        IEnumerator e = _repository.RegisterUser(msg.login, msg.pass, "");
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;

        Debug.Log("server register done");
        netMsg.conn.Send(MsgType.Highest + (short)NetMsgType.Register, new StringMessage(response));
    }
    void ClientConnect()
    {
        NetworkClient client = this.client;
        if (client == null)
        {
            client = StartClient();
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Login, OnClientLogin);
            client.RegisterHandler(MsgType.Highest + (short)NetMsgType.Register, OnClientRegister);
        }
    }
}
