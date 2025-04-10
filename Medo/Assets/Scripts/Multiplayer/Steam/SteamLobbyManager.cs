using Mirror;
using UnityEngine;
using Steamworks;
using TMPro;
using Mirror.FizzySteam;
using System;
//using fizzysteamworks;
//using System.Security.Cryptography;

public class SteamLobbyManager : MonoBehaviour
{

    public GameObject mainMenuUI;
    public GameObject lobbyUI;
    
    private const string LOBBY_NAME_KEY = "name";
    private CSteamID hostSteamID;
    private Callback<LobbyCreated_t> lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> joinRequest;
    private Callback<LobbyEnter_t> lobbyEntered;
    private CSteamID currentLobbyID; // Armazena o lobby atual pra convidar amigos
    public static CSteamID CurrentLobbyID { get; private set; } = CSteamID.Nil;
    public static bool HasActiveLobby => CurrentLobbyID != CSteamID.Nil;

    private void Start()
    {
        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);

        // Troca de UI
        mainMenuUI.SetActive(false);
        lobbyUI.SetActive(true);
    }



    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("[Steam] Falha ao criar o lobby Steam.");
            return;
        }

        currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        Debug.Log($"[Steam] Lobby criado com sucesso! ID: {currentLobbyID}");
        SteamLobbyManager.CurrentLobbyID = currentLobbyID;

        SteamMatchmaking.SetLobbyData(
            currentLobbyID,
            "name",
            SteamFriends.GetPersonaName()
        );

        NetworkManager.singleton.StartHost();

        // Fallback: adiciona player manualmente, se não foi criado ainda
        if (NetworkClient.localPlayer == null)
        {
            NetworkConnectionToClient conn = NetworkServer.localConnection;
            GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);
            Debug.Log("[Steam] Player adicionado manualmente ao host.");
        }
    }




    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
{
    CSteamID lobbyId = new CSteamID(callback.m_ulSteamIDLobby);
    CSteamID ownerId = SteamMatchmaking.GetLobbyOwner(lobbyId);
    CSteamID myId = SteamUser.GetSteamID();

    Debug.Log($"[Steam] Entrou no lobby {lobbyId}");
    Debug.Log($"[Steam] Host SteamID: {ownerId}");
    Debug.Log($"[Steam] Meu SteamID: {myId}");

    // Se for o host, não conecta como cliente
    if (myId == ownerId)
    {
        Debug.Log("[Steam] Eu sou o host, não conectando como cliente.");
        return;
    }

    string hostAddress = ownerId.ToString();
    Uri steamUri = new Uri("steam://" + hostAddress);
    NetworkManager.singleton.StartClient(steamUri);
    NetworkClient.OnConnectedEvent += () =>
    {
        Debug.Log("[Steam] Cliente conectado - enviando Ready e AddPlayer.");
        NetworkClient.Ready();
        NetworkClient.AddPlayer();
    };
    mainMenuUI.SetActive(false);
    lobbyUI.SetActive(true);
}



    public void InviteFriends()
    {
        if (currentLobbyID == CSteamID.Nil)
        {
            Debug.LogError("[Steam] Nenhum lobby ativo para convidar amigos.");
            return;
        }
        Debug.Log( $"currentLobbyID: {currentLobbyID}");
        SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
    }


    public void JoinLobby()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[SteamLobby] Saindo do jogo, limpando lobby...");

        // Exemplo: talvez sair do lobby?
        if (SteamManager.Initialized)
        {
            SteamMatchmaking.LeaveLobby(new CSteamID(/* ID atual do lobby */));
        }
    }

    private void OnDestroy()
    {
        if (lobbyCreated != null) lobbyCreated.Dispose();
        if (joinRequest != null) joinRequest.Dispose();
        if (lobbyEntered != null) lobbyEntered.Dispose();
    }


}
