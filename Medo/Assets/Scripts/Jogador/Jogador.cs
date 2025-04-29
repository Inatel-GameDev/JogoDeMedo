using System;
using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Classe principal de jogador
// Controlar a troca entre estados e possui as variaveis que precisam ser compartilhadas entre estados

// public class Jogador : NetworkBehaviour, MaquinaDeEstado
public class Jogador : NetworkBehaviour, MaquinaDeEstado
{
    
    // [SyncVar(hook = nameof(OnPlayerNameChanged))]
    private string playerName = "Player";
    public TextMeshProUGUI playerNameText;
 
    //[SerializeField] public GameManager manager;
    public Rigidbody rb;    
    [SerializeField] public SoundPlayer soundPlayer;
    // public Anim anim;
    
    [Header("Estados")]
    [SerializeField] public Estado EstadoAtivo;
    [SerializeField] public Estado EstadoParalisado;
    [SerializeField] public Estado EstadoAtual;
    // MiniTask     
    // RagDoll 
    
    [Header("Status")]
    [SerializeField] private float vida;
    [SerializeField] private float vidaMaxima;
    [SerializeField] public float velocidade;
    [SerializeField] public int capacidade;
    [SerializeField] public Inventario inventario;
    [SerializeField] public float veneno; 
    [SerializeField] private float resistencia; 
    [SerializeField] private float cooldownVeneno = 5f; 
    
    
    [Header("HUD")]
    [SerializeField] private TMP_Text textoVida;
    [SerializeField] private CameraPOV cameraPOV;
    [SerializeField] private RectTransform celular;
    [SerializeField] private bool celularPraCima = false;
    [SerializeField] private bool movendoCelular = false;
    [SerializeField] private GameObject textoVeneno;
    [SerializeField] private Image fillImage;

    
    // ****************************************************************************************************************
    // Unity

    private void Start()
    {
        // Manager
        // if (!isLocalPlayer && cameraPOV != null)
        //     cameraPOV.gameObject.SetActive(false);
        NetworkIdentity netId = GetComponentInParent<NetworkIdentity>();
        EstadoAtual = EstadoAtivo;
        textoVida.SetText("Saúde: " + vidaMaxima);
        EstadoAtual.Enter();
    }
    
    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        CmdSendPosition(transform.position);
        
        EstadoAtual.FixedDo();
    }
    
    // ****************************************************************************************************************
    // Estados e GamePlay
    
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
    
    public void machucaJogador(float dano){
        vida -= dano;
        if(vida <= 0){
            Morte();
        } else {            
            cameraPOV.TriggerDamageEffect();
            textoVida.SetText("Saúde: " + vida);
            soundPlayer.playSound(SoundsJogador.instance.atingido);
            Debug.Log("Machucado");            
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BombaFungo")){
            if (resistencia == 0)
            {
                CriaBarraVeneno();
            }
            Destroy(other.gameObject);
            veneno += 1; 
            StartCoroutine(Envenenado());
        }                
    }
    
    public IEnumerator Envenenado()
    {
        resistencia += veneno;
        fillImage.fillAmount = resistencia / 100;
        
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
        textoVida.SetText("Saúde: " + vidaMaxima);
        vida = vidaMaxima;
        soundPlayer.playSound(SoundsJogador.instance.morte);
        Debug.Log("Morreu");
    }

    // ****************************************************************************************************************
    // HUD
    
    private void CriaBarraVeneno()
    {
        textoVeneno.SetActive(true);
        fillImage.gameObject.SetActive(true);
    }
    
    public void MoveCelular()
    {
        Debug.Log("Move Celular");
        
        if(movendoCelular)
            return;
        StartCoroutine(MoveCelularCorrotina());
        
        celularPraCima = !celularPraCima;
    }

    public IEnumerator MoveCelularCorrotina()
    {
        Debug.Log("Movendo Celular");
        Vector2 target;
        if (celularPraCima)
        {
            target = new Vector2(-720,-550);
        }
        else
        {
            target = new Vector2(-720,-290);
        }

        while (Vector2.Distance(celular.anchoredPosition, target) > 0.1f)
        {
            movendoCelular = true;
            celular.anchoredPosition = Vector2.Lerp(
                celular.anchoredPosition,
                target,
                0.5f * Time.deltaTime
            );
        }
      
        movendoCelular = false;
        yield return new WaitForSeconds(0.1f);
    }
    
    public void PutNameOnUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
            Debug.Log("[PlayerController] Nome atualizado na UI: " + playerName);
        }
        else
        {
            Debug.LogWarning("[PlayerController] playerNameText não atribuído.");
        }
    }
    
    // ****************************************************************************************************************
    // Network
    public void SetPlayerName(string nome)
    {
        playerName = nome;
    }
    private void OnPlayerNameChanged(string oldName, string newName)
    {
        PutNameOnUI();
    }
    
    [Command]
    void CmdSendPosition(Vector3 pos)
    {
        RpcUpdatePosition(pos);
    }
    [ClientRpc]
    void RpcUpdatePosition(Vector3 pos)
    {
        if (isLocalPlayer) return;
        transform.position = pos;
    }
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        if (cameraPOV.gameObject != null)
        {
            cameraPOV.gameObject.SetActive(true);
            Debug.Log("[PlayerController] Câmera ativada.");
        }
    }

}