using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelectDisplay : NetworkBehaviour
{
    [SerializeField] private CharacterDataBase characterDataBase;
    [SerializeField] private Transform charactersHolder;
    [SerializeField] private CharacterSelectButton selectButtonPrefab;
    [SerializeField] private PlayerCard[] playerCards;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private GameObject characterInfoPanel;
    [SerializeField] private Transform introSpawnPoint;

    private GameObject introInstance;
    private List<CharacterSelectButton> characterButtons = new();

    private NetworkList<CharacterSelectState> players;
    private void Awake()
    {
        players = new NetworkList<CharacterSelectState>();
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
        if (IsClient)
        {
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
        }

        // if (IsHost)
        // {
        //     joinCodeText.text = LobbyManager.Instance.GetRelayCode();
        // }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChange;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;

        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new CharacterSelectState(clientId));
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
        characterNameText.text = character.DisplayName;
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
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectCharacterServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }



            players[i] = new CharacterSelectState(
                players[i].ClientId,
                characterId
            );
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
    }
}
