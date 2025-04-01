using System;
using UnityEngine;


// Classe principal de jogador
// Controlar a troca entre estados e possui as variaveis que precisam ser compartilhadas entre estados
public class Jogador : MaquinaDeEstado
{

    [SerializeField] public JogadorAndando EstadoAndando;
    // MiniTask 
    // Parado
    // RagDoll 

 
    [SerializeField] public GameManager manager;
    public Rigidbody rb;    
    // public Anim anim;
    [SerializeField] private float vida;
    [SerializeField] private float velocidade;
    [SerializeField] private float capacidade;

    //public Item[] inventario;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        EstadoAtual = EstadoAndando;
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

    public float getVelocidade(){
        return velocidade;
    }

    public void machucaJogador(float dano){
        vida -= dano;
        if(vida <= 0){
            Debug.Log("Morreu");
        } else {
            Debug.Log("Machucado");
        }
    }


}