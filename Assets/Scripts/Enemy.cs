using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]

public class Enemy : Unit
{

    [Header("Movement")]
    [SerializeField] float moveRadius = 10f;
    [SerializeField] float minMoveDelay = 4f;
    [SerializeField] float maxMoveDelay = 12f;
    Vector3 startPosition;
    Vector3 curDistanation;
    float changePosTime;

    [Header("Behavior")]
    [SerializeField] bool aggressive;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float reviveDelay = 5f;
    float reviveTime;

    void Start()
    {
        startPosition = transform.position;
        changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
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
        Wandering(Time.deltaTime);
    }

    protected override void Revive()
    {
        base.Revive();
        transform.position = startPosition;
        if (isServer)
        {
            motor.MoveToPoint(startPosition);
        }
    }

    void Wandering(float deltaTime)
    {
        changePosTime -= deltaTime;
        if (changePosTime <= 0)
        {
            RandomMove();
            changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        }
    }

    void RandomMove()
    {
        curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(moveRadius, 0, 0) + startPosition;
        motor.MoveToPoint(curDistanation);
    }
}
