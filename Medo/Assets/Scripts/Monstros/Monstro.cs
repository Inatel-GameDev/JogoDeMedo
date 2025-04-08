using UnityEngine;
using UnityEngine.AI;

public abstract class Monstro : MaquinaDeEstado
{
    [SerializeField] public GameManager manager;
    public Rigidbody rb;
    public NavMeshAgent agente;
    [SerializeField] public SoundPlayer soundPlayer;
    [SerializeField] public float dano;
    [SerializeField] public float vida;
    [SerializeField] public float armadura;
    
    // lista de jogadores 
    // sorteio entre eles 
    // mais proximo do monstro 

    public void machucaMonstro(float danoExterno)
    {
        danoExterno -= armadura;
        if(danoExterno <= 0)
            return;
        
        vida -= danoExterno;
    }

    public void Morte()
    {
        Debug.Log("Morte Monstro");
        Destroy(this.gameObject);
    }
}
