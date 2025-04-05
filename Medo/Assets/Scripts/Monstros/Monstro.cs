using UnityEngine;
using UnityEngine.AI;

public abstract class Monstro : MaquinaDeEstado
{
    [SerializeField] public GameManager manager;
    public Rigidbody rb;
    public NavMeshAgent agente;
    [SerializeField] public SoundPlayer soundPlayer;
    [SerializeField] public float dano;
}
