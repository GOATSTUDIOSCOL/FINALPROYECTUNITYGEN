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

        /*joinHostButton.onClick.AddListener(() =>
        {
            StartVivox();
        });*/
    }

    public async void StartVivox()
    {
        try
        {
            voicePanel.SetActive(true);
            await JoinLobbyChannel();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error initializing Vivox: " + ex.Message);
        }
    }

    async Task JoinLobbyChannel()
    {
        try
        {

            var joinedLobby = LobbyManager.Instance.GetJoinedLobby();
            if (joinedLobby != null)
            {
                await VivoxService.Instance.JoinGroupChannelAsync(joinedLobby.Name, ChatCapability.AudioOnly);
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
