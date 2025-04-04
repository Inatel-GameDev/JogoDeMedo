using System;
using System.Collections;
using UnityEngine;

public class Fungo : MaquinaDeEstado
{
    [SerializeField] private Estado estadoVagando;
    [SerializeField] private Estado estadoVagandoSemi;
    [SerializeField] private Estado estadoVagandoMatrix;
    [SerializeField] private GameObject bomba;
    [SerializeField] private float cooldownBomba = 2f;
    
    private void Start()
    {
        EstadoAtual = estadoVagandoMatrix;
        EstadoAtual.Enter();
        StartCoroutine("Ataque");
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

    IEnumerator  Ataque(){
        yield return new WaitForSeconds(cooldownBomba);
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;        
        Instantiate(bomba, pos,rot); 
        StartCoroutine("Ataque");                 
    }
}
