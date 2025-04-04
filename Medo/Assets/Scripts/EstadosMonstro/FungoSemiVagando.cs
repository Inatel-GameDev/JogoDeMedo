using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Full Aleatório 
public class FungoSemiVagando : Estado
{
    
    // public float wanderTimer;
    private Transform target;
    [SerializeField] private NavMeshAgent agent;

    public float wanderRadius = 10f;
    public float minDistanceBetweenPoints = 5f;
    public int maxHistorySize = 5;

    private List<Vector3> positionHistory = new List<Vector3>();
    private Vector3 currentDestination;

    void FindNewDestination()
    {
        Vector3 newDestination = FindValidPosition();
        
        if (newDestination != Vector3.negativeInfinity)
        {
            currentDestination = newDestination;
            agent.SetDestination(currentDestination);
            UpdatePositionHistory(currentDestination);
        }
    }

    Vector3 FindValidPosition()
    {
        Vector3 finalPosition = Vector3.negativeInfinity;
        bool positionFound = false;
        int attempts = 0;

        while (!positionFound && attempts < 30)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                if (IsPositionValid(hit.position))
                {
                    finalPosition = hit.position;
                    positionFound = true;
                }
            }
            attempts++;
        }
        return finalPosition;
    }

    bool IsPositionValid(Vector3 targetPosition)
    {
        foreach (Vector3 position in positionHistory)
        {
            if (Vector3.Distance(targetPosition, position) < minDistanceBetweenPoints)
            {
                return false;
            }
        }
        return true;
    }

    void UpdatePositionHistory(Vector3 newPosition)
    {
        positionHistory.Add(newPosition);
        
        if (positionHistory.Count > maxHistorySize)
        {
            positionHistory.RemoveAt(0);
        }
    }

    // Opcional: Visualizar no Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        Gizmos.color = Color.green;
        if (currentDestination != Vector3.zero)
        {
            Gizmos.DrawSphere(currentDestination, 0.5f);
        }
    }

    public override void Enter()
    {         
        //timer = wanderTimer;
        //agent = GetComponent<NavMeshAgent>();
        FindNewDestination();
    }

    public override void Do()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            FindNewDestination();
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

