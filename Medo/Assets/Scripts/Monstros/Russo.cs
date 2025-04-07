using System;
using UnityEngine;
using UnityEngine.AI;

public class Russo : Monstro
{
    public Estado estadoPerseguindo;
    public Estado estadoPerto;
    public Estado estadoAtacando;
    [SerializeField] public Jogador jogadorAlvo;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private float dist;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        EstadoAtual = estadoPerseguindo;
        EstadoAtual.Enter();
    }

    public override void Update()
    {
        EstadoAtual.Do();
    }

    public override void FixedUpdate()
    {
        EstadoAtual.FixedDo();
        
        if (jogadorAlvo != null && EstadoAtual != estadoAtacando)
        {
            dist = (jogadorAlvo.transform.position - transform.position).sqrMagnitude;
            
            if (dist < distanciaMinima && EstadoAtual != estadoPerto)
            {
                MudarEstado(estadoPerto);
            }
            if(dist>= distanciaMinima && EstadoAtual != estadoPerseguindo)
            {
                MudarEstado(estadoPerseguindo);
            }
        }
    }

    public override void LateUpdate()
    {
        EstadoAtual.LateDo();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && EstadoAtual == estadoPerto){
            {
                jogadorAlvo = other.gameObject.GetComponent<Jogador>();
                MudarEstado(estadoAtacando);
            }
        }        
    }
}
