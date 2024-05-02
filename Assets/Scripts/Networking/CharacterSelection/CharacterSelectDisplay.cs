using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectDisplay : NetworkBehaviour
{
    [SerializeField] private CharacterDataBase characterDataBase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private PlayerCard[] playerCards;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject characterInfoPanel;
    [SerializeField] private Transform introSpawnPoint;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject inventoryPanel;

    private GameObject introInstance;
    private readonly List<CharacterSelectButton> characterButtons = new();

    private NetworkList<CharacterSelectState> players;
    [SerializeField] private Button startGameButton;
    
    private void Awake()
    {
        players = new NetworkList<CharacterSelectState>();
        startGameButton.onClick.AddListener(() =>
        {
            StartGame();
        });
    }
    public void StartGame()
    {
        if (IsServer)
        {
            foreach (var client in players)
            {
                var character = characterDataBase.GetCharacterById(client.CharacterId);
                if (character != null)
                {
                    var spawnPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                    var characterInstance = Instantiate(character.PlayerPrefab, spawnPos, Quaternion.identity);
                    characterInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                    introInstance.SetActive(false);
                }
            }
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
            Debug.Log("Host Joined");

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
        Character[] allCharacters = characterDataBase.GetAllCharacters();

        foreach (Character character in allCharacters)
        {
            // In tutorial says var
            CharacterSelectButton selectButtonInstance = Instantiate(selectButtonPrefab, charactersHolder);
            selectButtonInstance.SetCharacter(this, character);
            characterButtons.Add(selectButtonInstance);
        }
        players.OnListChanged += HandlePlayersStateChange;
        Debug.Log("Client Joined");


        // if (IsHost)
        // {
        //     joinCodeText.text = LobbyManager.Instance.GetRelayCode();
        // }
    }

    public override void OnNetworkDespawn()
    {

        players.OnListChanged -= HandlePlayersStateChange;


        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;

        }
    }

    private void HandleClientConnected(ulong clientId)
    {

        if (!players.Contains(new CharacterSelectState(clientId)))
        {
            players.Add(new CharacterSelectState(clientId));
            SelectCharacter(characterDataBase.GetCharacterById(1));
        }
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }

    public void SelectCharacter(Character character)
    {
        characterNameText.text = LobbyManager.Instance.PlayerName();
        characterInfoPanel.SetActive(true);

        for (int i = 0; i < players.Count; i++)
        {

            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
        }

        if (introInstance != null)
        {
            Destroy(introInstance);
        }

        introInstance = Instantiate(character.IntroPrefab, introSpawnPoint);

        SelectCharacterServerRpc(character.Id);
        LobbyManager.Instance.UpdatePlayerCharacter(character.Id);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectCharacterServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }


            Debug.Log("Character ID: " + characterId);
            players[i] = new CharacterSelectState(
                players[i].ClientId,
                characterId
            );

            Debug.Log(players[i]);
            Debug.Log(playerCards.Length);
            Debug.Log("CCCCCCCCCCCCCCCCCCCCCCCCCCCC");

            if ((int)players[i].ClientId < playerCards.Length)
            {

                playerCards[i].UpdateDisplay(players[i]);
            }
        }

    }

    private void HandlePlayersStateChange(NetworkListEvent<CharacterSelectState> changeEvt)
    {

        for (int i = 0; i < playerCards.Length; i++)
        {
            if (players.Count > i)
            {
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }

        foreach (var player in players)
        {
            if (player.ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
            break;
        }
    }
}
