using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// Classe genérica para movimentação pelo mapa sem considerar o jogador 
/// --- Algoritmos implementados: Matriz, Aleatório sem meḿoria, Aleatório com memória curta
public class Errante : MonoBehaviour
{
    
// ******************************************************************************************************************
    
    // Matriz    

    [Header("Matrix")]
    public float densidade = 1f;    // Distância entre os pontos
    public float tamanho;
    private Vector3[,] gridMatrix;
    private int gridSizeX, gridSizeZ;
    [SerializeField] private int posX,posY;
    [SerializeField] private int dir;
    public void GenerateGridMatrix(Transform transformObj)
    {
        // Calcula o tamanho real do plano considerando a escala
        Vector3 scaledSize = new Vector3(
            tamanho * transformObj.localScale.x, 
            0f,
            tamanho * transformObj.localScale.z
        );

        // Calcula a quantidade de pontos em cada eixo
        gridSizeX = Mathf.FloorToInt(scaledSize.x / densidade) + 1;
        gridSizeZ = Mathf.FloorToInt(scaledSize.z / densidade) + 1;

        gridMatrix = new Vector3[gridSizeX, gridSizeZ];

        // Geração dos pontos
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                // Calcula a posição local
                Vector3 localPosition = new Vector3(
                    (-scaledSize.x / 2) + (x * densidade),
                    0f,
                    (-scaledSize.z / 2) + (z * densidade)
                );

                // Converte para posição mundial considerando rotação e posição do objeto
                gridMatrix[x, z] = transformObj.TransformPoint(localPosition);
            }
        }
    }
    
    public void FindNewDestinationMatrix(NavMeshAgent agent)
    {
        Vector3 newDestination =  gridMatrix[posX,posY];        
        agent.SetDestination(newDestination); 
        
        if(dir == 0){
            
            posX++;
    
            if(posX == gridSizeX-1){
                dir = -1;
                posY++;
            }  
        } else {
            
            posX--;
    
            if(posX == 0){
                dir = 0;
                posY++;
            }  
        }
        if(posY == gridSizeZ -1 ){
            // para e não faz mais nada ? 
            agent.isStopped = true;
        }
    }
    void OnDrawGizmos()
    {
        if(gridMatrix != null)
        {
            Gizmos.color = Color.cyan;
            foreach(Vector3 point in gridMatrix)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
    
    
// ******************************************************************************************************************
    
    // Aleatório sem Memória 
    [Header("Aleatório sem memória")]
    public float wanderRadius;
    public float wanderTimer;
    
    // implementação no Estado
    // if (timer >= wanderTimer) {
    //     Vector3 newPos = RandomNavSphere(transform.position);
    //     agent.SetDestination(newPos);
    //     timer = 0;
    // }
    public Vector3 RandomNavSphere(Vector3 origin) {
        Vector3 randDirection = Random.insideUnitSphere * wanderRadius;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition (randDirection, out navHit, wanderRadius, -1);

        return navHit.position;
    }
    
    
// ******************************************************************************************************************
    
    // Aleatório com Memória 
    
    [Header("Aleatório com memória")]
    
    public float wanderRadiusMemoria = 10f;
    public float minDistanceBetweenPoints = 5f;
    public int maxHistorySize = 5;
    private Vector3 currentDestination;
    private List<Vector3> positionHistory = new List<Vector3>();
    
    
    // if (agent.remainingDistance <= agent.stoppingDistance)
    // {
    //     FindNewDestination();
    // }
    void FindNewDestinationAleatorio(NavMeshAgent agent)
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
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadiusMemoria;
            randomDirection += transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadiusMemoria, NavMesh.AllAreas))
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
}
