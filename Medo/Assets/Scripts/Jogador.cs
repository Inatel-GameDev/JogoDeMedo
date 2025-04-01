using System;
using UnityEngine;


// Classe principal de jogador
// Controlar a troca entre estados e possuis as variaveis que precisam ser compartilhadas entre estados
public class Jogador : MaquinaDeEstado
{
    [SerializeField] public Estado EstadoAtual;

    [SerializeField] public JogadorAndando EstadoAndando;
 
    [SerializeField] public GameManager manager;
    public Rigidbody rb;    
    // public Anim anim;


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


}