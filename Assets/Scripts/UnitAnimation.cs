using UnityEngine;
using UnityEngine.AI;

public class UnitAnimation : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate () {
		if (!agent.hasPath) {
            animator.SetBool("Moving", false);
        } else {
            animator.SetBool("Moving", true);
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
