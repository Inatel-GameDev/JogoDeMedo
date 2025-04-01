using System;
using UnityEngine;

public abstract class MaquinaDeEstado : MonoBehaviour
{
    [SerializeField] public Estado EstadoAtual;

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
    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void LateUpdate();
    
}
