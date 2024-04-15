using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;

public class TestLobby : MonoBehaviour
{

    private Lobby hostLobby;
    private const float MAX_HEART_BEAT_TIME = 0.15f;
    private float currentHeartBeatTimer = 0;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartBeat();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "generation";
            int maxPlayers = 4;
            Lobby hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            Debug.Log("Created lobby: " + hostLobby.Name + " " + hostLobby.MaxPlayers);
        }
        catch (LobbyServiceException exp)
        {
            Debug.Log("Exception Message: " + exp.Message);
        }
    }

    public async void ListLobbies()
    {
        try
        {

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }

        }
        catch (LobbyServiceException exp)
        {
            Debug.Log("Exception Message: " + exp.Message);
        }
    }

    public async void HandleLobbyHeartBeat()
    {
        currentHeartBeatTimer += Time.deltaTime;
        if (currentHeartBeatTimer > MAX_HEART_BEAT_TIME)
        {
            if (hostLobby != null)
                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            currentHeartBeatTimer = 0;
        }
    }

    public async void JoinLobby()
    {
        try
        {

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);

            Debug.Log("Joined at lobby: " + queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException exp)
        {
            Debug.Log("Exception Message: " + exp.Message);
        }
    }
}
