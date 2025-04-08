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
        Debug.LogError("Falha ao criar o lobby Steam.");
        return;
    }

    Debug.Log("Lobby criado com sucesso! Iniciando host...");

    SteamMatchmaking.SetLobbyData(
        new CSteamID(callback.m_ulSteamIDLobby),
        "name",
        SteamFriends.GetPersonaName()
    );

    // Dentro de OnLobbyCreated
    NetworkManager.singleton.StartHost();

    // adiciona player manualmente se precisar
    if (NetworkClient.localPlayer == null)
    {
        NetworkConnectionToClient conn = NetworkServer.localConnection;
        GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }


    NetworkManager.singleton.StartHost();
}



    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return;

        string hostAddress = SteamMatchmaking.GetLobbyOwner(new CSteamID(callback.m_ulSteamIDLobby)).ToString();
        Uri steamUri = new Uri("steam://" + hostAddress);
        NetworkManager.singleton.StartClient(steamUri);

        mainMenuUI.SetActive(false);
        lobbyUI.SetActive(true);
    }


    public void JoinLobby()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }
}
