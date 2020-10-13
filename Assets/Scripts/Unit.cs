using UnityEngine;
using UnityEngine.Networking;

public class Unit : Interactable
{
    [SerializeField] protected UnitMotor motor;
    [SerializeField] protected UnitStats _stats;
    public UnitStats stats { get { return _stats; } }

    protected Interactable focus;
    protected bool isDie;

    public delegate void UnitDenegate();
    [SyncEvent] public event UnitDenegate EventOnDamage;
    [SyncEvent] public event UnitDenegate EventOnDie;
    [SyncEvent] public event UnitDenegate EventOnRevive;

    public override void OnStartServer()
    {
        motor.SetMoveSpeed(_stats.moveSpeed.GetValue());
        _stats.moveSpeed.onStatChanged += motor.SetMoveSpeed;
    }

    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnLiveUpdate() { }
    protected virtual void OnDieUpdate() { }

    protected void OnUpdate()
    {
        if (isServer)
        {
            if (isServer)
            {
                if (!isDie)
                {
                    if (_stats.curHealth == 0) Die();
                    else OnLiveUpdate();
                }
                else
                {
                    OnDieUpdate();
                }
            }
        }
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

    protected virtual void DamageWithCombat(GameObject user)
    {
        EventOnDamage();
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
        isDie = true;
        GetComponent<Collider>().enabled = false;
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
        isDie = false;
        if (isServer)
        {
            hasInteract = true;
            _stats.SetHealthRate(1);
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
