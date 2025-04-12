using Mirror;
using UnityEngine;
using Steamworks;
using System;

public class SteamLobbyManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject lobbyUI;

    private const string LOBBY_NAME_KEY = "name";
    private CSteamID currentLobbyID;
    public static CSteamID CurrentLobbyID { get; private set; } = CSteamID.Nil;
    public static bool HasActiveLobby => CurrentLobbyID != CSteamID.Nil;

    private Callback<LobbyCreated_t> lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> joinRequest;
    private Callback<LobbyEnter_t> lobbyEntered;

    // ✅ Proteção contra AddPlayer duplo
    private bool jaAdicionouPlayer = false;

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
        CurrentLobbyID = currentLobbyID;

        Debug.Log($"[Steam] Lobby criado com sucesso! ID: {currentLobbyID}");

        SteamMatchmaking.SetLobbyData(currentLobbyID, LOBBY_NAME_KEY, SteamFriends.GetPersonaName());

        CustomNetworkManager manager = (CustomNetworkManager)NetworkManager.singleton;
        manager.StartHost();

        NetworkClient.Ready();

        if (!jaAdicionouPlayer && !NetworkClient.localPlayer)
        {
            jaAdicionouPlayer = true;
            Debug.Log("[Steam] Host adicionando player.");
            NetworkClient.AddPlayer();
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

        if (myId == ownerId)
        {
            Debug.Log("[Steam] Eu sou o host, não conectando como cliente.");
            return;
        }

        Uri steamUri = new Uri("steam://" + ownerId);
        NetworkManager.singleton.StartClient(steamUri);

        NetworkClient.OnConnectedEvent += () =>
        {
            Debug.Log("[Steam] Cliente conectado - enviando Ready e AddPlayer.");
            NetworkClient.Ready();

            // ✅ Protege contra instanciar o LobbyPlayer duas vezes
            if (!jaAdicionouPlayer && !NetworkClient.localPlayer)
            {
                jaAdicionouPlayer = true;
                Debug.Log("[Steam] Cliente adicionando player.");
                NetworkClient.AddPlayer();
            }
            else
            {
                Debug.LogWarning("[Steam] AddPlayer ignorado (já adicionado ou player existente).");
            }
        };

        mainMenuUI.SetActive(false);
        lobbyUI.SetActive(true);
    }

    public void InviteFriends()
    {
        if (CurrentLobbyID == CSteamID.Nil)
        {
            Debug.LogError("[Steam] Nenhum lobby ativo para convidar amigos.");
            return;
        }

        Debug.Log($"[Steam] currentLobbyID: {CurrentLobbyID}");
        SteamFriends.ActivateGameOverlayInviteDialog(CurrentLobbyID);
    }

    public void JoinLobby()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[SteamLobby] Saindo do jogo, limpando lobby...");

        if (SteamManager.Initialized && SteamLobbyManager.HasActiveLobby)
        {
            Debug.Log("[Steam] Saindo do lobby antes de desligar a API...");
            SteamMatchmaking.LeaveLobby(SteamLobbyManager.CurrentLobbyID);
        }
    }

    private void OnDestroy()
    {
        lobbyCreated?.Dispose();
        joinRequest?.Dispose();
        lobbyEntered?.Dispose();
    }
}
