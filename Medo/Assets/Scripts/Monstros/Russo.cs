using System;
using System.Collections;
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
    [SerializeField] private float tempoTiro = 5f;
    public bool pausa = false;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        EstadoAtual = estadoPerseguindo;
        EstadoAtual.Enter();
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
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && EstadoAtual == estadoPerto && !pausa){
            {
                jogadorAlvo = other.gameObject.GetComponent<Jogador>();
                MudarEstado(estadoAtacando);
            }
        }        
    }

    public void Pausa()
    {
        StartCoroutine(corrotinaPausa());
    }

    private IEnumerator corrotinaPausa()
    {
        pausa = true;
        yield return new WaitForSeconds(tempoTiro);
        MudarEstado(estadoPerseguindo);
        pausa = false;
    }
}
