using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RussoAtacando : Estado
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public Russo monstro;
    

    public override void Enter()
    {
        Debug.Log("RussoAtacando");
        // girar jogador 
        // girar monstro 
        canvas.gameObject.SetActive(true);
        
        monstro.jogadorAlvo.MudarEstado(monstro.jogadorAlvo.EstadoParalisado);
    }

    public override void Exit()
    {
    }

    public override void FixedDo()
    {
        
    }
    

}
