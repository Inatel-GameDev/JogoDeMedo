using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FungoMatrix : Estado
{
    [SerializeField] private Errante errante;
    [SerializeField] private NavMeshAgent agent;
    
    public override void Enter()
    {
       errante.GenerateGridMatrix(transform);
       errante.FindNewDestinationMatrix(agent);
       
    }
    
    public override void FixedDo()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            errante.FindNewDestinationMatrix(agent);
        }
    }

    public override void Exit()
    {
        
    }

}