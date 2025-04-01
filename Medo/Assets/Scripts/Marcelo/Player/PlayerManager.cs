using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;

public class PlayerManager : NetworkSingleton<PlayerManager>
{

    private NetworkVariable<int> playerInGame = new NetworkVariable<int>();
    public int PlayersInGame
    {
        get { return playerInGame.Value; }
    }
    
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if(IsServer)
            {
                playerInGame.Value++;
                Debug.Log("Player connected: " + id + " Players in game: " + playerInGame.Value);
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if(IsServer)
            {
                playerInGame.Value--;
                Debug.Log("Player disconnected: " + id + " Players in game: " + playerInGame.Value);
            }
        };
    }
    

}
