using System;
using UnityEngine;
using UnityEngine.AI;

public class RussoPerseguino : Estado
{
    [SerializeField] private Transform jogador;
    [SerializeField] private Russo monstro;
    [SerializeField] private AudioSource audioAndando;
    [SerializeField] private Vector3 posicao;
    [SerializeField] private float tempoDeAtraso;
    [SerializeField] private float tempoAtual;
    [SerializeField] private bool posicaoCerta;
    [SerializeField] private Errante erranteAlg;


    public void pegaPosicaoAtrasada()
    {
        if(monstro.jogadorAlvo != null)
            posicao = monstro.jogadorAlvo.transform.position;
        else
        {
            Debug.Log("Jogaddor nulo !!!");
        }
    }
    
    public void pegaPosicaoAtrasadaComErro()
    {
        // to do -> trocar pra um aleatorio da lista ou o mais perto 
        posicao = erranteAlg.RandomNavSphere(jogador.transform.position);
    }
    
    
    public override void Enter()
    {
        //audioAndando.Play();
        tempoAtual = 0;
    }
    
    public override void FixedDo()
    {
        if (tempoAtual > tempoDeAtraso)
        {
            if (posicaoCerta)
            {
                pegaPosicaoAtrasada();
            }
            else
            {
                pegaPosicaoAtrasadaComErro();
            }
            tempoAtual = 0;
            monstro.agente.destination = posicao;
        }
        else
        {
            tempoAtual += Time.fixedDeltaTime;   
        }
    }
    
    public override void Exit()
    {
        monstro.agente.isStopped = true;
        audioAndando.Stop();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            posicaoCerta = true;
            monstro.jogadorAlvo = collider.gameObject.GetComponent<Jogador>();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Jogador>() == monstro.jogadorAlvo)
        {
            posicaoCerta = false;
            monstro.jogadorAlvo = null;
        }
    }
}
