using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monstro : MonoBehaviour, MaquinaDeEstado
{
    [SerializeField] public GameManager manager;
    [SerializeField] public Estado EstadoAtual;
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

    public void MudarEstado(Estado novoEstado)
    {
        {
            try
            {
                EstadoAtual.Exit();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            EstadoAtual = novoEstado;
            EstadoAtual.Enter();
        }
    }

    public abstract void FixedUpdate();
    
    
}
