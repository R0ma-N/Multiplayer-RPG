using UnityEngine;
using UnityEngine.Networking;

sealed class UnitLoader : NetworkBehaviour
{
    [SerializeField] GameObject unitPrefab;
    [SerializeField] PlayerController controller;

    [SyncVar(hook = "HookUnitIdentity")] NetworkIdentity unitIdentity;

    public override void OnStartAuthority()
    {
        if (isServer)
        {
            GameObject unit = Instantiate(unitPrefab);
            NetworkServer.Spawn(unit);
            unitIdentity = unit.GetComponent<NetworkIdentity>();
            controller.SetCharacter(unit.GetComponent<Character>(), true);
        }
        else
        {
            CmdCreatePlayer();
        }
    }

    [Command]
    public void CmdCreatePlayer()
    {
        GameObject unit = Instantiate(unitPrefab);
        NetworkServer.Spawn(unit);
        unitIdentity = unit.GetComponent<NetworkIdentity>();
        controller.SetCharacter(unit.GetComponent<Character>(), false);
    }

    [ClientCallback]
    void HookUnitIdentity(NetworkIdentity unit)
    {
        if (isLocalPlayer)
        {
            unitIdentity = unit;
            controller.SetCharacter(unit.GetComponent<Character>(), true);
        }
    }
}

