using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { get; private set; }
    private NetworkVariable<int> keys = new NetworkVariable<int>();
    private const int initialKeys = 0;

    [SerializeField] private TextMeshProUGUI keysText;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // public override void OnNetworkSpawn()
    // {
    //     if (IsServer)
    //     {
    //         keys.Value = initialKeys;
    //         NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    //     }
    //     else
    //     {
    //         if (keys.Value != initialKeys)
    //         {
    //             Debug.LogWarning($"NetworkVariable was {keys.Value} upon being spawned" +
    //                 $" when it should have been {initialKeys}");
    //         }
    //         else
    //         {
    //             Debug.Log($"NetworkVariable is {keys.Value} when spawned.");
    //         }
    //         keys.OnValueChanged += OnKeyValueChanged;
    //     }
    // }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("Client Connected");
    }

    private void OnKeyValueChanged(int previous, int current)
    {
        Debug.Log($"Detected NetworkVariable Change: Previous: {previous} | Current: {current}");
        keysText.text = "Keys: " + current.ToString();
    }

    [Rpc(SendTo.Server)]
    public void AddKeyRpc()
    {
        keys.Value++;
        keysText.text = "Keys: " + keys.Value.ToString();
    }
}
