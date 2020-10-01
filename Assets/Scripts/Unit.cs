using UnityEngine;
using UnityEngine.Networking;

public class Unit : Interactable
{
    [SerializeField] protected UnitMotor motor;
    [SerializeField] protected UnitStats myStats;

    protected Interactable focus;
    protected bool isDead;

    public delegate void UnitDenegate();
    [SyncEvent] public event UnitDenegate EventOnDamage;
    [SyncEvent] public event UnitDenegate EventOnDie;
    [SyncEvent] public event UnitDenegate EventOnRevive;

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

    public override bool Interact(GameObject user)
    {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null)
        {
            if (combat.Attack(myStats))
            {
                EventOnDamage();
                return true;
            }
        }
        return base.Interact(user);
    }

    protected virtual void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
    }

    protected virtual void RemoveFocus()
    {
        focus = null;
        motor.StopFollowingTarget();
    }

    [ClientCallback]
    protected virtual void Die()
    {
        isDead = true;
        if (isServer)
        {
            hasInteract = false;
            RemoveFocus();
            motor.MoveToPoint(transform.position);
            EventOnDie();
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
            hasInteract = true;
            myStats.SetHealthRate(1);
            EventOnRevive();
            RpcRevive();
        }
    }

    [ClientRpc]
    void RpcRevive()
    {
        if (!isServer) Revive();
    }
}
