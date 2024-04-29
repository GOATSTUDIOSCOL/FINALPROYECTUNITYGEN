using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set;}
    public event EventHandler<EventArgs> OnPlayerDead;
    public event EventHandler<SpawnPlayerEventArgs> OnSpawnOrDespawnPlayer;
    public class SpawnPlayerEventArgs : EventArgs
    {
        public Dictionary<ulong, GameObject> cameras;
    }

    private void Awake() {
        Instance = this;
    }

    public void PlayerDead() {
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnOrDespawnPlayer(Dictionary<ulong, GameObject> cameras) {
        OnSpawnOrDespawnPlayer?.Invoke(this, new SpawnPlayerEventArgs{cameras = cameras});
    }
}
