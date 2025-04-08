using System;
using System.Collections;
using TMPro;
using UnityEngine;


// Classe principal de jogador
// Controlar a troca entre estados e possui as variaveis que precisam ser compartilhadas entre estados
public class Jogador : MaquinaDeEstado
{

    [SerializeField] public Estado EstadoAndando;
    [SerializeField] public Estado EstadoParalisado;
    // MiniTask 
    // Parado
    // RagDoll 

 
    [SerializeField] public GameManager manager;
    public Rigidbody rb;    
    // public Anim anim;
    [SerializeField] private float vida;
    [SerializeField] private float vidaMaxima;
    [SerializeField] private float velocidade;
    [SerializeField] private float capacidade;
    [SerializeField] private TMP_Text textoVida;
    [SerializeField] public SoundPlayer soundPlayer;
    [SerializeField] public float veneno; 
    [SerializeField] private float resistencia; 
    [SerializeField] private float cooldownVeneno = 5f; 
    [SerializeField] private CameraPOV cameraPOV; 

    //public Item[] inventario;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        EstadoAtual = EstadoAndando;
        textoVida.SetText(vida + "/" + vidaMaxima);
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
            Morte();
        } else {            
            cameraPOV.TriggerDamageEffect();
            textoVida.SetText(vida + "/" + vidaMaxima);
            soundPlayer.playSound(SoundsJogador.instance.atingido);
            Debug.Log("Machucado");            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BombaFungo")){
            Destroy(other.gameObject);
            veneno += 1; 
            StartCoroutine(Envenenado());
        }
    }

    public IEnumerator Envenenado()
    {
        resistencia += veneno;
        yield return new WaitForSeconds(cooldownVeneno);
        if (resistencia >= 100)
        {
            Morte();
        }
        else
        {
            StartCoroutine(Envenenado());    
        }
    }

    public void Morte()
    {
        textoVida.SetText(vida + "/" + vidaMaxima);
        soundPlayer.playSound(SoundsJogador.instance.morte);
        Debug.Log("Morreu");
    }
}