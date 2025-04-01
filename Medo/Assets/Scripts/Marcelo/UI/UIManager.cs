using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TextMeshProUGUI playerInGameText;

    private void awake()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        startClientButton.onClick.AddListener(() => 
        { 
            if(NetworkManager.Singleton.StartClient())
            {
                //Logger.Instance.LogInfo("Client started...");
                Debug.Log("Client started...");
            }
            else
            {
                //Logger.Instance.LogInfo("Failed to start client...");
                Debug.Log("Failed to start client...");
            }
         });
        startHostButton.onClick.AddListener(() => 
        {
            if(NetworkManager.Singleton.StartHost())
            {
                //Logger.Instance.LogInfo("Host started...");
                Debug.Log("Host started...");
            }
            else
            {
                //Instance.LogInfo("Failed to start host...");
                Debug.Log("Failed to start host...");
            }
         });
        startServerButton.onClick.AddListener(() => 
        { 
            if(NetworkManager.Singleton.StartServer())
            {
                //Logger.Instance.LogInfo("Server started...");
                Debug.Log("Server started...");
            }
            else
            {
                //Logger.Instance.LogInfo("Failed to start server...");
                Debug.Log("Failed to start server...");
            }
         });
    }

    private void update()
    {
        //playerInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
    }

}
