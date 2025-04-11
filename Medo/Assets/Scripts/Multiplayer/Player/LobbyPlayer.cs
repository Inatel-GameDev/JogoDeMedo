using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    public Camera playerCamera;


    public override void OnStartServer()
    {
        base.OnStartServer();

        if (SteamManager.Initialized)
        {
            playerName = SteamFriends.GetPersonaName();
        }
        else
        {
            playerName = "Player " + netId;
        }
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
    public override void OnStartAuthority()
    {
        DontDestroyOnLoad(gameObject); // Persiste até trocar pelo player real
        base.OnStartAuthority();
        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);
    }
}
