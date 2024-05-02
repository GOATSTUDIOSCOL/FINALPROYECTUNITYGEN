using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
using Unity.Services.Vivox;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    private const float MAX_HEART_BEAT_TIME = 15f;
    private const float LOBBY_POLL_TIMER_MAX = 5f;
    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_CHARACTER = "Character";
    public const string KEY_START_GAME = "CodeForStart";

    public const string KEY_SPAWNING_PLAYER = "SpawningPlayer";


    public event EventHandler OnLeftLobby;
    public event EventHandler OnCompletedPlayers;
    public event EventHandler<UserErrorEventArgs> OnLobbyUserError;
    public class UserErrorEventArgs : EventArgs
    {
        public string errorMessage;
    }


    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public class LobbyEventArgs : EventArgs
    {
        public Lobby lobby;
    }


    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }

    public event EventHandler OnAuthenticationLobby;
    public event EventHandler OnInAuthenticatedScreen;
    public event EventHandler OnChooseCharacter;

    private float heartbeatTimer = 0;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private Lobby joinedLobby;
    private string playerName;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Enable when release the final product
        // HandleRefreshLobbyList();
        // HandleLobbyHeartBeat();
        HandleLobbyPolling();
    }

    public async void Authentication(string playerName)
    {
        this.playerName = playerName;
        try {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(playerName);
        
            await UnityServices.InitializeAsync(initializationOptions);

            AuthenticationService.Instance.SignedIn += () =>
            {
                RefreshLobbyList();
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            await VivoxService.Instance.InitializeAsync();
            OnAuthenticationLobby?.Invoke(this, new EventArgs{});
        } catch (AuthenticationException exc) {
            OnLobbyUserError?.Invoke(this, new UserErrorEventArgs { errorMessage = exc.Message });
        }
        
    }

    public async void LoginToVivoxAsync()
    {
        if (VivoxService.Instance.IsLoggedIn)
        {
            LoginOptions options = new LoginOptions();
            options.DisplayName = playerName;
            options.EnableTTS = false;
            await VivoxService.Instance.LoginAsync(options);
        }
    }


    public async void LogoutBasicServices() {
        try {

            if (VivoxService.Instance.IsLoggedIn) {
                await VivoxService.Instance.LogoutAsync();            
            }
            LeaveLobby();  
            AuthenticationService.Instance.SignOut(true);

        } catch (LobbyServiceException exp) {
            Debug.Log(exp.Message);
        }
    }

    private void HandleRefreshLobbyList()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn)
        {
            refreshLobbyListTimer -= Time.deltaTime;
            if (refreshLobbyListTimer < 0f)
            {
                float refreshLobbyListTimerMax = 5f;
                refreshLobbyListTimer = refreshLobbyListTimerMax;

                RefreshLobbyList();
            }
        }
    }

    public async void CreateLobby(string lobbyName, int maxPlayers = 4, bool isPrivate = false)
    {
        if (lobbyName == null) {
            OnLobbyUserError?.Invoke(this, new UserErrorEventArgs { errorMessage = "Choose a valid lobby name to continue, it can not be empty." });
            return;
        }

        try {
            Player player = GetPlayer();

            CreateLobbyOptions options = new()
            {
                Player = player,
                IsPrivate = isPrivate,
                Data = new Dictionary<string, DataObject> {
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0")}
                },
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            joinedLobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });

            string relayCode = await RelayManager.Instance.CreateRelay();

            await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                        {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)},
                        {KEY_PLAYER_CHARACTER, new DataObject(DataObject.VisibilityOptions.Member, "1")}
                    }
            });

            Debug.Log("Created Lobby " + lobby.Name + " players " + lobby.MaxPlayers);

            VivoxAuthentication.Instance.StartVivox();
            NetworkManager.Singleton.StartHost();
        } catch (Exception exc) {
            Debug.Log("JOIN LOBBY ERRO");
            OnLobbyUserError?.Invoke(this, new UserErrorEventArgs { errorMessage = exc.Message });
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
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }

        }
        catch (LobbyServiceException exp)
        {
            Debug.Log("Exception Message: " + exp.Message);
        }
    }

    public async void HandleLobbyHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = MAX_HEART_BEAT_TIME;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;
            Debug.Log("Quick Join Lobby");
            VivoxAuthentication.Instance.StartVivox();
            bool joinerelayEvent = await RelayManager.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
            if (joinerelayEvent)
            {
                NetworkManager.Singleton.StartClient();
            }
            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }
        catch (LobbyServiceException exp)
        {
            Debug.Log("Excepcion Message: " + exp.Message);
        }
    }

    private Player GetPlayer()
    {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            { KEY_PLAYER_CHARACTER, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "1") },

        });
    }

    public async void UpdatePlayerName(string playerName)
    {
        this.playerName = playerName;

        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new()
                {
                    Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        KEY_PLAYER_NAME, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: playerName)
                    }
                }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
            }
            catch (LobbyServiceException e)
            {

                Debug.Log("This is the error   ");
                Debug.Log(e);
            }
        }
    }

    public async void UpdatePlayerCharacter(int characterId)
    {

        if (joinedLobby != null)
        {
            try
            {
                UpdatePlayerOptions options = new()
                {
                    Data = new Dictionary<string, PlayerDataObject>()
                    {
                        {
                            KEY_SPAWNING_PLAYER, new PlayerDataObject(
                                visibility: PlayerDataObject.VisibilityOptions.Public,
                                value: characterId.ToString())
                        }
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
                joinedLobby = lobby;

                OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs
                {
                    lobby = joinedLobby
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void RefreshLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new()
            {
                Count = 10,

                Filters = new List<QueryFilter> {
                    new(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0")
                },

                Order = new List<QueryOrder> {
                    new(
                        asc: false,
                        field: QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobby(Lobby lobby)
    {
        try
        {
            Player player = GetPlayer();

            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
            {
                Player = player
            });
            VivoxAuthentication.Instance.StartVivox();

            if (joinedLobby.Data.ContainsKey(KEY_START_GAME))
            {
                bool joinerelayEvent = await RelayManager.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                if (joinerelayEvent)
                {
                    NetworkManager.Singleton.StartClient();
                }
            }

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("Error joining lobby " + e);
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                if (joinedLobby.HostId ==  AuthenticationService.Instance.PlayerId) {
                    Debug.Log("Left Host");
                } else {
                    Debug.Log("Left Client");
                }
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                joinedLobby = null;
                OnLeftLobby?.Invoke(this, EventArgs.Empty);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public Lobby GetJoinedLobby()
    {
        return joinedLobby;
    }

    public bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    private async void HandleLobbyPolling()
    {
        if (joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f)
            {

                lobbyPollTimer = LOBBY_POLL_TIMER_MAX;

                var updatedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                if (updatedLobby != null)
                {
                    joinedLobby = updatedLobby;

                    OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                    if (!IsPlayerInLobby())
                    {
                        Debug.Log("Kicked from Lobby!");

                        OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

                        joinedLobby = null;
                    }
                    // else
                    // {
                    //     if (joinedLobby.Data.ContainsKey(KEY_START_GAME) && joinedLobby.Data[KEY_START_GAME].Value != "0")
                    //     {
                    //         if (!IsLobbyHost())
                    //         {
                    //             bool initRelayClient = await RelayManager.Instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);
                    //         }
                    //         // joinedLobby = null;
                    //         // Hide();
                    //     }
                    // }
                }
            }
        }
    }


    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (Player player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public string GetRelayCode()
    {
        return GetJoinedLobby().Data[KEY_START_GAME].Value;
    }

    public string PlayerName() => playerName;

    public string GetSpawingPlayer()
    {
        return GetJoinedLobby().Data[KEY_SPAWNING_PLAYER].Value;
    }

    // private void Show()
    // {
    //     gameObject.SetActive(true);
    // }

}
