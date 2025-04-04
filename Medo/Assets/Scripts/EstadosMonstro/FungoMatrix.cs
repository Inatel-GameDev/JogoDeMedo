using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FungoMatrix : Estado
{
    [Header("Configurações do Grid")]
    public float densidade = 1f; // Distância entre os pontos
    public bool drawGizmos = true; // Visualização no Editor
    [SerializeField] private float tamanho;

    [SerializeField] private Vector3[,] gridMatrix;
    private int gridSizeX, gridSizeZ;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private int posX,posY;
    [SerializeField] private int dir;

    public void GenerateGridMatrix()
    {
        // Calcula o tamanho real do plano considerando a escala
        Vector3 scaledSize = new Vector3(
            tamanho * transform.localScale.x, 
            0f,
            tamanho * transform.localScale.z
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
                gridMatrix[x, z] = transform.TransformPoint(localPosition);
            }
        }
    }

    // Para visualização no Editor
    void OnDrawGizmos()
    {
        if(drawGizmos && gridMatrix != null)
        {
            Gizmos.color = Color.cyan;
            foreach(Vector3 point in gridMatrix)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }


    void FindNewDestination()
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
        }
    }

    public override void Enter()
    {
        GenerateGridMatrix();
        Debug.Log(gridMatrix.Length);
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