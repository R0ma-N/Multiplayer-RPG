﻿using System;
using UnityEngine;
using UnityEngine.Networking;

public class Unit : Interactable
{
    public event Action EventOnDamage;
    public event Action EventOnDie;
    public event Action EventOnRevive;

    [SerializeField] protected UnitMotor _motor;
    [SerializeField] protected UnitStats _stats;
    protected bool _isDead;
    protected Interactable _focus;

    public UnitStats Stats
    {
        get
        {
            return _stats;
        }
    }

    void Update()
    {
        OnUpdate();
    }
    protected virtual void OnAliveUpdate() { }
    protected virtual void OnDeadUpdate() { }
    protected void OnUpdate()
    {
        if (isServer)
        {
            if (!_isDead)
            {
                if (_stats.CurHealth == 0)
                {
                    Die();
                }
                else
                {
                    OnAliveUpdate();
                }
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        if (!isServer) Die();
    }
    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) Revive();
    }
    protected virtual void Die()
    {
        _isDead = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer)
        {
            HasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }
    protected virtual void Revive()
    {
        _isDead = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer)
        {
            HasInteract = true;
            _stats.SetHealthRate(1);
            RpcRevive();
        }
    }
    protected virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != _focus)
        {
            _focus = newFocus;
            _motor.FollowTarget(newFocus);
        }
    }
    protected virtual void RemoveFocus()
    {
        _focus = null;
        _motor.StopFollowingTarget();
    }
    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(_stats))
            {
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }
    public override void OnStartServer()
    {
        _motor.SetMoveSpeed(_stats.MoveSpeed.GetValue());
        _stats.MoveSpeed.OnStatChanged += _motor.SetMoveSpeed;
    }
    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
    }
}
