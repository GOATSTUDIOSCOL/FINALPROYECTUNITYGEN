using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { get; private set; }
    private NetworkVariable<int> keys = new NetworkVariable<int>();
    private NetworkVariable<float> timeLeft = new NetworkVariable<float>();
    public float initialTime = 15 * 60;
    private const int initialKeys = 0;
    public TextMeshProUGUI timeUIText;

    [SerializeField] private TextMeshProUGUI keysText;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateCounterRpc(timeLeft.Value -= Time.deltaTime);

            if (timeLeft.Value <= 0)
            {
                Debug.Log("Game Lost!");
            }
        }
    }
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            keys.Value = initialKeys;
            timeLeft.Value = initialTime;
            NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }
        else
        {
            if (keys.Value != initialKeys)
            {
                Debug.LogWarning($"NetworkVariable was {keys.Value} upon being spawned" +
                    $" when it should have been {initialKeys}");
            }
            else
            {
                Debug.Log($"NetworkVariable is {keys.Value} when spawned.");
            }
            keys.OnValueChanged += OnKeyValueChanged;
        }
    }

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

    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateCounterRpc(float time)
    {
        timeLeft.Value = time;
        timeUIText.text = FormatTime(timeLeft.Value);
    }

    string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
