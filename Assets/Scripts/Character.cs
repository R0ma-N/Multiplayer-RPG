using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit
{
    [SerializeField] float reviveDelay = 5f;
    [SerializeField] GameObject gfx;

    Vector3 startPosition;
    float reviveTime;
    public Inventory inventory;

    void Start()
    {
        startPosition = transform.position;
        reviveTime = reviveDelay;
    }

    void Update()
    {
        OnUpdate();
    }

    protected override void OnDeadUpdate()
    {
        base.OnDeadUpdate();
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

    protected override void OnAliveUpdate()
    {
        base.OnAliveUpdate();
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
    
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.dropPoint = transform;
    }

    public void SetMovePoint(Vector3 point)
    {
        if (!isDead)
        {
            RemoveFocus();
            motor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus)
    {
        if (!isDead)
        {
            if (newFocus.hasInteract) SetFocus(newFocus);
        }
    }
}
