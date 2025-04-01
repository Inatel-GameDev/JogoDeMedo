using System;
using UnityEngine;
using UnityEngine.AI;

public class Monstro : MaquinaDeEstado
{
    [SerializeField] public Estado EstadoPerseguindo;
    [SerializeField] public GameManager manager;
    public Rigidbody rb;
    public NavMeshAgent agente;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private float dano;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agente = GetComponent<NavMeshAgent>();
        EstadoAtual = EstadoPerseguindo;
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
            soundPlayer.playSound(SoundsLerdo.instance.explosao);
            Jogador jogador = other.gameObject.GetComponent<Jogador>();
            jogador.machucaJogador(dano);
            Debug.Log("Explodir");
        }        
    }
}
