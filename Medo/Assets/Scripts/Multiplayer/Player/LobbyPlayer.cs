using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    public Camera playerCamera;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        DontDestroyOnLoad(gameObject);

        // Ativa câmera se necessário
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        // Envia o nome pro servidor
        if (SteamManager.Initialized)
        {
            string nome = SteamFriends.GetPersonaName();
            CmdSetPlayerName(nome);
        }
    }

    [Command]
    void CmdSetPlayerName(string nome)
    {
        playerName = nome;
        Debug.Log("[Mirror] Nome recebido no servidor: " + playerName);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(WaitForName());
        Debug.Log("LobbyPlayer entrou no cliente: " + playerName);
    }

    private System.Collections.IEnumerator WaitForName()
    {
        while (string.IsNullOrEmpty(playerName))
            yield return null;

        Debug.Log("Adicionando player com nome: " + playerName);
        Debug.Log("UI Manager: " + LobbyUIManager.Instance);
        LobbyUIManager.Instance.AddPlayerToList(this);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        LobbyUIManager.Instance.RemovePlayerFromList(this);
    }
}
