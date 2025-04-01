using System;
using UnityEngine;
using UnityEngine.AI;

public class Monstro : MaquinaDeEstado
{
    [SerializeField] public Estado estadoPerseguindo;
    [SerializeField] public Estado estadoAtcando;
    [SerializeField] public GameManager manager;
    public Rigidbody rb;
    public NavMeshAgent agente;
    [SerializeField] public SoundPlayer soundPlayer;
    [SerializeField] public float dano;
    //public Jogador jogadorAlvo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        EstadoAtual = estadoPerseguindo;
        EstadoAtual.Enter();
    }

    public override void Update()
    {
        EstadoAtual.Do();
    }

    public override void FixedUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.P)) MudarEstado(DriveEstado);
        EstadoAtual.FixedDo();
    }

    public override void LateUpdate()
    {
        EstadoAtual.LateDo();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){                        
            //jogadorAlvo = other.gameObject.GetComponent<Jogador>();
            if(EstadoAtual == estadoPerseguindo)
                MudarEstado(estadoAtcando);
        }        
    }
}
