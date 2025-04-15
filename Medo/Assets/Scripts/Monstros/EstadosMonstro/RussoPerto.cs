using UnityEngine;
using UnityEngine.AI;

public class RussoPerto : Estado
{
    [SerializeField] private Russo monstro;
    [SerializeField] private AudioSource audioAndando;
    
    public override void Enter()
    {
        audioAndando.Play();
        monstro.agente.isStopped = false;
    }
    public override void FixedDo()
    {
        monstro.agente.destination = monstro.jogadorAlvo.transform.position;
    }
         
    public override void Exit()
    {
        monstro.agente.isStopped = true;
        audioAndando.Stop();

    }

}