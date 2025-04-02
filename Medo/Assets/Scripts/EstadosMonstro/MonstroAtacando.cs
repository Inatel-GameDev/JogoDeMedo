using System.Collections;
using UnityEngine;

public class MonstroAtacando : Estado
{
    [SerializeField] private Lerdo monstro;
    [SerializeField] private SphereCollider ataqueCollider;
    [SerializeField] private bool acertou = false;

    public override void Do()
    {
        
    }

    public override void Enter()
    {
        //monstro.soundPlayer.playSound(SoundsLerdo.instance.aviso);
        acertou = false;
        StartCoroutine("Ataque");        
    }

    public override void Exit()
    {
        if(!acertou)
            monstro.agente.speed += 1;
        
        if(monstro.agente.speed == 3)
            ataqueCollider.radius = 6;
        
        if(monstro.agente.speed == 5)
            ataqueCollider.radius = 7;
        
        if(monstro.agente.speed >= 7){
            ataqueCollider.radius = 8;
            monstro.agente.speed = 7;
        }

        if(acertou && monstro.agente.speed > 1){
            monstro.agente.speed--;
        }
        
    }

    public override void FixedDo()
    {
        
    }

    public override void LateDo()
    {
        
    }

    IEnumerator  Ataque(){
        yield return new WaitForSeconds(1);
        monstro.soundPlayer.playSound(SoundsLerdo.instance.aviso);
        yield return new WaitForSeconds(1);
        monstro.soundPlayer.playSound(SoundsLerdo.instance.explosao);
        ataqueCollider.enabled = true;       
        yield return new WaitForSeconds(0.3f);
        ataqueCollider.enabled = false;
        yield return new WaitForSeconds(1);
        monstro.MudarEstado(monstro.estadoPerseguindo);        
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){                        
            Debug.Log("acertou");
            Jogador jogadorAlvo = other.gameObject.GetComponent<Jogador>();
            jogadorAlvo.machucaJogador(monstro.dano);
            acertou = true;
        }        
    }

}
