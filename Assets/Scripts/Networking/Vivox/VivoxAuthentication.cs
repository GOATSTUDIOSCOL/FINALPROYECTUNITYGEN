using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

public class VivoxAuthentication : MonoBehaviour
{
    [SerializeField] private Button joinHostButton;
    [SerializeField] private GameObject voicePanel;
    public static VivoxAuthentication Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public async void StartVivox()
    {
        try
        {
            LobbyManager.Instance.LoginToVivoxAsync();
            voicePanel.SetActive(true);
            await JoinLobbyChannel();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing Vivox: " + ex.Message);
        }
    }

    Channel3DProperties props = new Channel3DProperties();
    async Task JoinLobbyChannel()
    {
        try
        {

            var joinedLobby = LobbyManager.Instance.GetJoinedLobby();
            if (joinedLobby != null)
            {
                await VivoxService.Instance.JoinGroupChannelAsync(joinedLobby.Name, ChatCapability.AudioOnly);
               //await VivoxService.Instance.JoinPositionalChannelAsync(joinedLobby.Name, ChatCapability.AudioOnly, props);
            }
            else
            {
                Debug.LogError("Joined lobby is null. Cannot join Vivox channel.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error joining Vivox channel: " + ex.Message);
        }
    }
}
