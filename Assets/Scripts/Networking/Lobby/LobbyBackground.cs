using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;


public class LobbyBackground : MonoBehaviour
{


    private void Awake()
    {
    }

    private void Start()
    {
        LobbyManager.Instance.OnInAuthenticatedScreen += LobbyManager_OnInAuthenticatedScreen;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnInAuthenticatedScreen;
        LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnOutAuthenticatedScreen;
    }

    private void LobbyManager_OnInAuthenticatedScreen(object sender, EventArgs e)
    {
        Show();
    }

    private void LobbyManager_OnOutAuthenticatedScreen(object sender, EventArgs e)
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

}