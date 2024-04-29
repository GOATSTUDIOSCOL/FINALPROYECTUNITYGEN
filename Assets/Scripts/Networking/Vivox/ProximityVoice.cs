using Unity.Services.Vivox;
using UnityEngine;

public class ProximityVoice : MonoBehaviour
{
    private float nextPosUpdate; 
    private GameObject playerGO;
    private float updateInterval = 0.3f;
    public string channelName;

    void Start()
    {
        nextPosUpdate = Time.time;
        playerGO = this.gameObject;
        channelName = LobbyManager.Instance.GetJoinedLobby().Name;
    }

    void Update()
    {
        if (Time.time > nextPosUpdate)
        {
            // Set player position in the positional channel
            VivoxService.Instance.Set3DPosition(playerGO, channelName);

            // Update next position update time
            nextPosUpdate = Time.time + updateInterval;
        }
    }
}
