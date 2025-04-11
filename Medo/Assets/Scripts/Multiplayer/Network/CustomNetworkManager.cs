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
        Debug.Log("[Mirror] OnServerAddPlayer chamado. Cena atual: " + SceneManager.GetActiveScene().name);
        string scene = SceneManager.GetActiveScene().name;

        if (scene.Contains("Lobby"))
        {
            GameObject lobbyPlayer = Instantiate(playerPrefab);
            Debug.Log("[Mirror] Instanciando LobbyPlayer e adicionando ao servidor...");
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        else
        {
            // Evita erro se for chamado prematuramente
            if (conn.identity == null)
            {
                Debug.LogWarning("[Mirror] conn.identity está null. Provavelmente o jogador ainda não foi adicionado.");
                return;
            }

            Debug.Log("[Mirror] Cena real, instanciando Player...");
            GameObject gamePlayer = Instantiate(spawnPrefabs.Find(p => p.name == "Player"));

            LobbyPlayer lobby = conn.identity.GetComponent<LobbyPlayer>();
            PlayerController controller = gamePlayer.GetComponent<PlayerController>();

            if (controller != null && lobby != null)
            {
                controller.SetNome(lobby.playerName);
            }

            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, ReplacePlayerOptions.KeepAuthority);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == "Marcelo") // Sua cena de jogo real
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn != null && conn.identity != null)
                {
                    LobbyPlayer lobby = conn.identity.GetComponent<LobbyPlayer>();
                    if (lobby != null)
                    {
                        GameObject player = Instantiate(spawnPrefabs.Find(p => p.name == "Player"));
                        PlayerController controller = player.GetComponent<PlayerController>();

                        if (controller != null)
                        {
                            controller.SetNome(lobby.playerName);
                        }

                        NetworkServer.ReplacePlayerForConnection(conn, player, ReplacePlayerOptions.KeepAuthority);
                    }
                }
            }
        }
    }
}
