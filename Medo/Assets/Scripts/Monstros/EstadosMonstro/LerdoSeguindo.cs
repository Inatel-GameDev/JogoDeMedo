using UnityEngine;
using UnityEngine.AI;

public class LerdoSeguindo : Estado
{
    [SerializeField] private Transform jogador;
    [SerializeField] private Lerdo monstro;
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
         
    public override void Exit()
    {
        monstro.agente.isStopped = true;
        audioAndando.Stop();

    }

}
