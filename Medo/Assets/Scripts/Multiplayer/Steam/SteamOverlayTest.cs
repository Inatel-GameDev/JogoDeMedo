using UnityEngine;
using Steamworks;

public class SteamOverlayTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SteamManager.Initialized)
            {
                Debug.Log("Tentando abrir o overlay via F1...");
                SteamFriends.ActivateGameOverlay("Friends");
            }
            else
            {
                Debug.LogWarning("Steam não inicializado.");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SteamManager.Initialized)
            {
                Debug.Log("Abrindo tela de convite...");
                // Mude 'currentLobbyID' para seu ID real se quiser testar o invite
                CSteamID fakeLobbyId = (CSteamID)123456789; 
                SteamFriends.ActivateGameOverlayInviteDialog(fakeLobbyId);
            }
        }
    }
}
