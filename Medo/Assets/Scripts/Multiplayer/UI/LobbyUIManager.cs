using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance;

    public GameObject playerEntryPrefab;
    public Transform playerListParent;
    public GameObject startGameButton;

    private Dictionary<NetworkIdentity, GameObject> entries = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPlayerToList(LobbyPlayer player)
    {
        NetworkIdentity id = player.netIdentity;

        if (entries.ContainsKey(id)) return;

        GameObject entry = Instantiate(playerEntryPrefab, playerListParent);
        TMP_Text nameText = entry.GetComponentInChildren<TMP_Text>();
        nameText.text = player.playerName;

        entries[id] = entry;
        UpdateStartButtonVisibility();
    }

    public void RemovePlayerFromList(LobbyPlayer player)
    {
        NetworkIdentity id = player.netIdentity;

        if (entries.ContainsKey(id))
        {
            Destroy(entries[id]);
            entries.Remove(id);
        }

        UpdateStartButtonVisibility();
    }

    void UpdateStartButtonVisibility()
    {
        if (startGameButton != null)
        {
            startGameButton.SetActive(NetworkServer.active);
        }
    }

    public void OnStartGamePressed()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.ServerChangeScene("Marcelo"); // 🔁 sua cena real
        }
    }
}
