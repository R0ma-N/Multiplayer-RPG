﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
sealed class UnitMotor : MonoBehaviour {

    NavMeshAgent agent;
    UnitAnimation animation;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponentInChildren<UnitAnimation>();
	}
	
	public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }

    public void Attack()
    {
        animation.Attack1();
    }
}
