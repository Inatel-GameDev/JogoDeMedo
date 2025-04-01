using UnityEngine;
using UnityEngine.AI;

public class MonstroPerseguindo : Estado
{
    [SerializeField] private Transform jogador;
    [SerializeField] private Monstro monstro;
    [SerializeField] private AudioSource audioAndando;
    
    public override void Enter()
    {
        audioAndando.Play();
        monstro.agente.isStopped = false;
    }
    public override void FixedDo()
    {
        monstro.agente.destination = jogador.position;
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

}
