using UnityEngine;
using UnityEngine.AI;

sealed class UnitAnimation : MonoBehaviour {
    private const string Moving = "Moving";
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void FixedUpdate () {
		if (!agent.hasPath) {
            animator.SetBool(Moving, false);
        } else {
            animator.SetBool(Moving, true);
        }
    }

    public void Attack1()
    {
        animator.SetTrigger("Attack");
    }

    //Placeholder functions for Animation events
    public void Hit() {
        
    }

    void FootR() {
    }

    void FootL() {
    }
}
