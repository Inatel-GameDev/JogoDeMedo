using System.Collections;
using UnityEngine;

public class MonstroAtacando : Estado
{
    [SerializeField] private Monstro monstro;
    [SerializeField] private Collider ataqueCollider;

    public override void Do()
    {
        
    }

    public override void Enter()
    {
        monstro.soundPlayer.playSound(SoundsLerdo.instance.aviso);
        StartCoroutine("Ataque");
    }

    public override void Exit()
    {
        
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
        }        
    }

}
