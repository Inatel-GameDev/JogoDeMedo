using UnityEngine;
using UnityEngine.AI;

public class MonstroPerseguindo : Estado
{
    [SerializeField] private Transform jogador;
    [SerializeField] private Monstro monstro;
    
    public override void Enter()
    {
        
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
        
    }

}
