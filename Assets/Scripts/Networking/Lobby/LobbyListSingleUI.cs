using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private Button joinLobbyBtn;


    private Lobby lobby;


    private void Awake()
    {
        joinLobbyBtn.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinLobby(lobby);
           // VivoxAuthentication.Instance.StartVivox();
        });
    }

    public void UpdateLobby(Lobby lobby)
    {
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
    }


}