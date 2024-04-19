using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerPreferences : MonoBehaviour
{
    public static EditPlayerPreferences Instance { get; private set; }
    private string playerName;

    private void Awake()
    {
        Instance = this;
    }


    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        LobbyManager.Instance.UpdatePlayerName(playerName);
    }

    public string GetPlayerName()
    {
        return playerName;
    }


}