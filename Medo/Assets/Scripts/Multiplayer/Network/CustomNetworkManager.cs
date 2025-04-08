using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Estamos na cena de lobby, instanciamos LobbyPlayer
        if (SceneManager.GetActiveScene().name == offlineScene)
        {
            GameObject lobbyPlayer = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        // Estamos na cena do jogo (fase real)
        else
        {
            GameObject gamePlayer = Instantiate(spawnPrefabs.Find(p => p.name == "Player"));


            LobbyPlayer lobby = conn.identity.GetComponent<LobbyPlayer>();
            PlayerController controller = gamePlayer.GetComponent<PlayerController>();

            if (controller != null && lobby != null)
                controller.SetNome(lobby.playerName);

            // Substitui o player do lobby pelo real
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, ReplacePlayerOptions.KeepAuthority);

        }
    }

    public override void OnServerSceneChanged(string sceneName)
{
    base.OnServerSceneChanged(sceneName);

    if (sceneName == "Marcelo") // Troque pelo nome da fase real
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                LobbyPlayer lobby = conn.identity.GetComponent<LobbyPlayer>();

                if (lobby != null)
                {
                    GameObject player = Instantiate(spawnPrefabs.Find(p => p.name == "Player"));

                    PlayerController controller = player.GetComponent<PlayerController>();
                    if (controller != null)
                        controller.SetNome(lobby.playerName);

                    NetworkServer.ReplacePlayerForConnection(conn, player, ReplacePlayerOptions.KeepAuthority);
                }
            }
        }
    }
}


}
