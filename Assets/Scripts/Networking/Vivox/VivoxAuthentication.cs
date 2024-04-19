using System.Threading.Tasks;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;

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
        joinHostButton.onClick.AddListener(() =>
        {
            LoginToVivox();

        });
    }

    public async void LoginToVivox()
    {
        await VivoxService.Instance.InitializeAsync();
        voicePanel.SetActive(true);
        await JoinLobbyChannel();
    }

    Task JoinLobbyChannel()
    {
        Debug.Log(LobbyManager.Instance.GetJoinedLobby().Name);
        return VivoxService.Instance.JoinGroupChannelAsync(LobbyManager.Instance.GetJoinedLobby().Name, ChatCapability.AudioOnly);
    }
}
