using System;
using UnityEngine;


// Classe principal de jogador
// Controlar a troca entre estados e possuis as variaveis que precisam ser compartilhadas entre estados
public class Jogador : MonoBehaviour
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

    private void Update()
    {
        EstadoAtual.Do();
    }

    private void FixedUpdate()
    {
        // if (Input.GetKeyDown(KeyCode.P)) MudarEstado(DriveEstado);
        EstadoAtual.FixedDo();
    }

    private void LateUpdate()
    {
        EstadoAtual.LateDo();
    }

    public void MudarEstado(Estado novoEstado)
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

    public void Pause()
    {
    }

    public void ResumePlay()
    {
    }


}