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
        Debug.Log($"[Steam] Entrou no lobby {lobbyId} | Owner: {SteamMatchmaking.GetLobbyOwner(lobbyId)}");

        if (NetworkServer.active) return; // host não precisa conectar como client

        string hostAddress = SteamMatchmaking.GetLobbyOwner(lobbyId).ToString();
        Uri steamUri = new Uri("steam://" + hostAddress);
        NetworkManager.singleton.StartClient(steamUri);

        mainMenuUI.SetActive(false);
        lobbyUI.SetActive(true);
    }

    public void InviteFriends()
    {
        if (SteamManager.Initialized)
            SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
    }


    public void JoinLobby()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }
}
