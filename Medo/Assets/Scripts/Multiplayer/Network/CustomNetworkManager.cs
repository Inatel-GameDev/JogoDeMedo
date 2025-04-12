using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public override void OnApplicationQuit()
    {
        if (NetworkServer.active)
        {
            Debug.Log("[Mirror] Encerrando como Host...");
            StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            Debug.Log("[Mirror] Encerrando como Client...");
            StopClient();
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        string scene = SceneManager.GetActiveScene().name;
        Debug.Log($"[Mirror] OnServerAddPlayer chamado. Cena atual: {scene}");

        if (scene.Contains("Lobby"))
        {
            GameObject lobbyPlayer = Instantiate(playerPrefab);
            Debug.Log("[Mirror] Instanciando LobbyPlayer e adicionando ao servidor...");
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        else
        {
            Debug.LogWarning("[Mirror] Cena real detectada em AddPlayer. Ignorando instanciamento...");
            // Não instancia nada — troca de player é feita manualmente pelo LobbyPlayer
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == "Marcelo")
        {
            Debug.Log("[Mirror] Cena de jogo carregada no servidor.");
            // A troca de player ocorre no LobbyPlayer após NetworkClient.ready
        }
    }
}
