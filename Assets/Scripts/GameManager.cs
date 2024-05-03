using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { get; private set; }
    private NetworkVariable<int> keys = new NetworkVariable<int>();
    public NetworkVariable<bool> shadowPuzle = new NetworkVariable<bool>();
    private NetworkVariable<float> timeLeft = new NetworkVariable<float>();
    public float initialTime = 15 * 60;
    public float localTime;
    private const int initialKeys = 0;
    public TextMeshProUGUI timeUIText;
    public GameObject losePanel;
    public GameObject winPanel;
    public bool gameStarted = false;
    public bool globalShadowState = false;
    private bool isPaused;
    public bool isAudioDevicesDisplayOpen = false;
    private GameObject pausePanel;
    public Canvas vivoxCanvas;
    public Door slidePuzzleDoor;
    public Door mainDoor;

    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private TextMeshProUGUI keysGoalText;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
        isPaused = false;
        pausePanel = GameObject.FindGameObjectWithTag("PauseMessage");
        pausePanel.SetActive(isPaused);

    }

    private void Update()
    {
        if (gameStarted && IsHost)
        {
            localTime = timeLeft.Value - Time.deltaTime;
            UpdateCounterRpc(localTime);

            if (timeLeft.Value <= 0)
            {
                losePanel.SetActive(true);
            }
        }
        if (gameStarted && Input.GetKeyDown(KeyCode.P) && pausePanel != null)
        {
            ChangePauseState();
            mainDoor.OpenDoorRpc();
        }
    }

    public void ChangePauseState()
    {
        if (isPaused)
        {
            if (!isAudioDevicesDisplayOpen)
            {
                HideCursor();
                vivoxCanvas.enabled = false;
                NetworkManager net = NetworkManager.Singleton;
                net.LocalClient.PlayerObject.GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>().enabled = true;
                isPaused = !isPaused;
            }
        }
        else
        {
            EnableCursor();
            vivoxCanvas.enabled = true;
            NetworkManager net = NetworkManager.Singleton;
            net.LocalClient.PlayerObject.GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>().enabled = false;
            isPaused = !isPaused;
        }
        pausePanel.SetActive(isPaused);
    }
    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void setAudioDevicesDisplay(bool value)
    {
        isAudioDevicesDisplayOpen = value;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            shadowPuzle.Value = false;
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
            timeLeft.OnValueChanged += OnTimeValueChanged;
            shadowPuzle.OnValueChanged += OnShadowValueChanged;
        }
    }

    private void OnShadowValueChanged(bool previousValue, bool newValue)
    {
        shadowPuzle.Value = true;
    }

    private void OnTimeValueChanged(float previousValue, float newValue)
    {
        timeUIText.text = FormatTime(newValue);
        if (newValue <= 0)
            losePanel.SetActive(true);
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        Debug.Log("Client Connected");
    }

    private void OnKeyValueChanged(int previous, int current)
    {
        Debug.Log($"Detected NetworkVariable Change: Previous: {previous} | Current: {current}");
        keysText.text = current.ToString();
        keysGoalText.text = keys.Value.ToString() + "/8";
        if (current <= 8)
        {
            mainDoor.OpenDoorRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void AddKeyRpc()
    {
        keys.Value++;
        keysText.text = keys.Value.ToString();
        keysGoalText.text = keys.Value.ToString() + "/8";
        if (keys.Value <= 8)
        {
            mainDoor.OpenDoorRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void UpdateCounterRpc(float time)
    {
        if (time > 0)
        {
            timeLeft.Value = time;
            timeUIText.text = FormatTime(time);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    [Rpc(SendTo.Server)]
    public void UpdateShadowRpc()
    {
        shadowPuzle.Value = true;
    }



    string FormatTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
