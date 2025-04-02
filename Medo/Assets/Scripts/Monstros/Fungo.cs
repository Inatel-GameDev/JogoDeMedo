using System;
using UnityEngine;

public class Fungo : MaquinaDeEstado
{
    [SerializeField] private Estado estadoVagando;
    
    private void Start()
    {
        EstadoAtual = estadoVagando;
    }

    public override void Update()
    {
        EstadoAtual.Do();
    }

    public override void FixedUpdate()
    {
        EstadoAtual.FixedDo();
    }

    public override void LateUpdate()
    {
        EstadoAtual.LateDo();
    }
}
