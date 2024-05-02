using UnityEngine;
using UnityEngine.UI;
using System;

public class Authentication : MonoBehaviour
{

    [SerializeField] private Button authenticateButton;


    private void Awake()
    {

        authenticateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.Authentication(EditPlayerPreferences.Instance.GetPlayerName());
        });

    }

    private void Start() {
        LobbyManager.Instance.OnAuthenticationLobby += LobbyManager_OnAuthenticationLobby;
    }

    public void LobbyManager_OnAuthenticationLobby(object sender, EventArgs e) {
        Hide();
    }  

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
