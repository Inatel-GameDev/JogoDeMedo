using System;
using UnityEngine;
using UnityEngine.AI;

public class RussoPerseguino : Estado
{
    //[SerializeField] private Transform jogador;
    [SerializeField] private Russo monstro;
    [SerializeField] private AudioSource audioAndando;
    
    [SerializeField] private bool posicaoAtrasada;


    public void pegaPosicaoAtrasada()
    {
        
    }
    
    public void pegaPosicaoAtrasadaComErro()
    {
        
    }
    
    
    public override void Enter()
    {
        //audioAndando.Play();
    }
    
    public override void FixedDo()
    {
        if (posicaoAtrasada)
        {
            pegaPosicaoAtrasada();
        }
        else
        {
            pegaPosicaoAtrasadaComErro();
        }
    }
    
    public override void Do()
    {
        
    }
    public override void LateDo()
    {
        
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
            posicaoAtrasada = true;
            monstro.jogadorAlvo = collider.gameObject.GetComponent<Jogador>();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Jogador>() == monstro.jogadorAlvo)
        {
            posicaoAtrasada = false;
            monstro.jogadorAlvo = null;
        }
    }
}
