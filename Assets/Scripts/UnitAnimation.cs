using UnityEngine;
using UnityEngine.AI;

sealed class UnitAnimation : MonoBehaviour {
    private const string Move = "Move";
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void FixedUpdate () {
		if (!agent.hasPath) {
            animator.SetBool(Move, false);
        } else {
            animator.SetBool(Move, true);
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
