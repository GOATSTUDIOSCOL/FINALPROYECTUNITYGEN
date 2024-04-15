using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class TestLobby : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "generation";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            Debug.Log("Created lobby: " + lobby.Name + " " + lobby.MaxPlayers);
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
}
