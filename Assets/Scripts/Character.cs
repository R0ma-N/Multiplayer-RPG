using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] float reviveDelay = 5f;
    [SerializeField] GameObject gfx;

    Vector3 startPosition;
    float reviveTime;
    public Player player;

    new public PlayerStats stats { get { return _stats as PlayerStats; } }


    void Start()
    {
        startPosition = transform.position;
        reviveTime = reviveDelay;
    }

    void Update()
    {
        OnUpdate();
    }

    protected override void OnDieUpdate()
    {
        base.OnDieUpdate();
        if (reviveTime > 0)
        {
            reviveTime -= Time.deltaTime;
        }
        else
        {
            reviveTime = reviveDelay;
            Revive();
        }
    }

    protected override void OnLiveUpdate()
    {
        base.OnLiveUpdate();
        if (focus != null)
        {
            if (!focus.hasInteract)
            {
                RemoveFocus();
            }
            else
            {
                float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);
                if (distance <= focus.radius)
                {
                    focus.Interact(gameObject);
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        gfx.SetActive(false);
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = startPosition;
        gfx.SetActive(true);
        if (isServer)
        {
            motor.MoveToPoint(startPosition);
        }
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!isDie)
        {
            RemoveFocus();
            motor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!isDie)
        {
            if (newFocus.hasInteract) SetFocus(newFocus);
        }
    }
}
