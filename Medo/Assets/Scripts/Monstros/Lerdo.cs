using System;
using UnityEngine;
using UnityEngine.AI;

public class Lerdo : Monstro
{
    [SerializeField] public Estado estadoSeguindo;
    [SerializeField] public Estado estadoAtacando;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        EstadoAtual = estadoSeguindo;
        EstadoAtual.Enter();
    }
    
    public override void FixedUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.P)) MudarEstado(DriveEstado);
        EstadoAtual.FixedDo();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){                        
            //jogadorAlvo = other.gameObject.GetComponent<Jogador>();
            if(EstadoAtual == estadoSeguindo)
                MudarEstado(estadoAtacando);
        }        
    }
}
