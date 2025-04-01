using System;
using UnityEngine;
using UnityEngine.AI;

public class Monstro : MaquinaDeEstado
{
    [SerializeField] public Estado EstadoAtual;
    [SerializeField] public Estado EstadoPerseguindo;
    [SerializeField] public GameManager manager;
    public Rigidbody rb;
    public NavMeshAgent agente;
    [SerializeField] private SoundPlayer soundPlayer;

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

    public override void MudarEstado(Estado novoEstado)
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
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            soundPlayer.playSound(Sounds.instance.explosao);
            Debug.Log("Explodir");
        }        
    }
}
