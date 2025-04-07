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
            float dist = (jogadorAlvo.transform.position - transform.position).sqrMagnitude;
            Debug.Log(dist);
            if (dist < distanciaMinima)
            {
                MudarEstado(estadoPerto);
            }
            else
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
        if(other.gameObject.CompareTag("Player")){    
            Debug.Log("ataque");
            //jogadorAlvo = other.gameObject.GetComponent<Jogador>();
            if(EstadoAtual == estadoPerto)
                MudarEstado(estadoAtacando);
        }        
    }
}
