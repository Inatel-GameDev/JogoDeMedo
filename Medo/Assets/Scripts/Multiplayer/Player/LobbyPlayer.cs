using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using System.Collections;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    public Camera playerCamera;

    private bool trocou = false;

    [SerializeField]
    private string sceneToChange;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // ⚠️ Evita instanciar LobbyPlayer na cena errada (por bug de timing)
        if (SceneManager.GetActiveScene().name == sceneToChange)
        {
            Debug.LogWarning("[LobbyPlayer] Spawnado na cena errada. Destruindo...");
            Destroy(gameObject);
        }
    }


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        if (SteamManager.Initialized)
        {
            string nome = SteamFriends.GetPersonaName();
            CmdSetPlayerName(nome);
        }

        SceneManager.sceneLoaded += OnSceneChanged;
    }

    [Command]
    private void CmdSetPlayerName(string nome)
    {
        playerName = nome;
        Debug.Log("[Mirror] Nome recebido no servidor: " + playerName);
    }


    private IEnumerator WaitForName()
    {
        while (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("[LobbyPlayer] Esperando nome syncar...");
            yield return null;
        }

        Debug.Log("Adicionando player com nome: " + playerName);
        if (LobbyUIManager.Instance != null)
            LobbyUIManager.Instance.AddPlayerToList(this);
    }


    public override void OnStopClient()
    {
        base.OnStopClient();

        if (LobbyUIManager.Instance != null)
            LobbyUIManager.Instance.RemovePlayerFromList(this);

        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;

        if (SceneManager.GetActiveScene().name == sceneToChange)
            Debug.Log("[LobbyPlayer] Destruído após troca de cena.");
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (!isOwned) return;

        if (scene.name == sceneToChange)
        {
            Debug.Log($"[LobbyPlayer] Cena {sceneToChange} carregada. Esperando estar pronto...");
            StartCoroutine(EsperarReadyAntesDeTrocar());
        }
    }

    private IEnumerator EsperarReadyAntesDeTrocar()
    {
        while (!NetworkClient.ready || string.IsNullOrEmpty(playerName))
            yield return null;

        yield return new WaitForSeconds(0.2f); // buffer extra pra sync da Steam

        if (trocou)
        {
            Debug.LogWarning("[LobbyPlayer] Já trocou, ignorando.");
            yield break;
        }

        trocou = true;
        Debug.Log("[LobbyPlayer] Client está ready e nome recebido, solicitando troca de player...");
        CmdRequestPlayerSwap();
    }


    [Command]
    private void CmdRequestPlayerSwap()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("[LobbyPlayer] Nome ainda vazio na hora da troca. Abortando.");
            return;
        }

        Debug.Log("[LobbyPlayer] CmdRequestPlayerSwap chamado");
        StartCoroutine(SwapPlayerCoroutine());
    }

    private IEnumerator SwapPlayerCoroutine()
    {
        if (!NetworkManager.singleton.spawnPrefabs.Exists(p => p.name == "Player"))
        {
            Debug.LogError("[Mirror] Prefab 'Player' não está registrado em spawnPrefabs!");
            yield break;
        }

        var gamePlayerPrefab = NetworkManager.singleton.spawnPrefabs.Find(p => p.name == "Player");
        GameObject newPlayer = Instantiate(gamePlayerPrefab);

        Jogador controller = newPlayer.GetComponent<Jogador>();
        if (controller != null)
        {
            controller.SetPlayerName(playerName); // 🔁 seta nome replicável
        }

        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, ReplacePlayerOptions.KeepAuthority);

        yield return null;

        NetworkServer.Destroy(gameObject);
    }



    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(CheckIfShouldDestroy());
    }

    private IEnumerator CheckIfShouldDestroy()
    {
        // 🔁 Aguarda nome sincronizar e valida se está na cena errada
        while (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("[LobbyPlayer] Esperando nome syncar...");
            yield return null;
        }

        if (SceneManager.GetActiveScene().name == sceneToChange)
        {
            Debug.LogWarning($"[LobbyPlayer] Spawnou em {sceneToChange} com nome " + playerName + ". Destruindo...");
            Destroy(gameObject);
            yield break;
        }

        Debug.Log("Adicionando player com nome: " + playerName);
        LobbyUIManager.Instance?.AddPlayerToList(this);
    }


}
