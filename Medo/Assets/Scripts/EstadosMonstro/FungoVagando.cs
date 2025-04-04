using UnityEngine;
using UnityEngine.AI;

// Full Aleatório 
public class FungoVagando : Estado
{
    public float wanderRadius;
    public float wanderTimer;
    private Transform target;
    [SerializeField] private NavMeshAgent agent;
    private float timer;

    public Vector3 RandomNavSphere(Vector3 origin) {
        Vector3 randDirection = Random.insideUnitSphere * wanderRadius;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition (randDirection, out navHit, wanderRadius, -1);

        return navHit.position;
    }

    public override void Enter()
    {         
        timer = wanderTimer;
    }

    public override void Do()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public override void FixedDo()
    {
        
    }

    public override void LateDo()
    {
        
    }

    public override void Exit()
    {
        
    }
}

