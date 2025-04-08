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
}
