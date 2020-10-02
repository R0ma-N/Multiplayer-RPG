using UnityEngine;
using UnityEngine.Networking;

public class Unit : NetworkBehaviour
{
    [SerializeField] protected UnitMotor motor;
    [SerializeField] protected UnitStats myStats;

    protected bool isDead;

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
            if (!isDead)
            {
                if (myStats.curHealth == 0) Die();
                else OnAliveUpdate();
            }
            else
            {
                OnDeadUpdate();
            }
        }
    }

    [ClientCallback]
    protected virtual void Die()
    {
        isDead = true;
        if (isServer)
        {
            motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie()
    {
        if (!isServer) Die();
    }

    [ClientCallback]
    protected virtual void Revive()
    {
        isDead = false;
        if (isServer)
        {
            myStats.SetHealthRate(1);
            RpcRevive();
        }
    }

    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) Revive();
    }
}
