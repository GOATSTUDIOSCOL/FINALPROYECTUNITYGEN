using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI message;
    

    private void Start()
    {
        LobbyManager.Instance.OnLobbyUserError += LobbyManager_OnLobbyUserError;
        gameObject.SetActive(false);
    }

    public void LobbyManager_OnLobbyUserError(object sender, LobbyManager.UserErrorEventArgs e) {
        this.message.text = e.errorMessage;
        gameObject.SetActive(true);
    }  
}