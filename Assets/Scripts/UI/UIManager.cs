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
        public Dictionary<ulong, Transform> heads;
        public ulong networkIdObj;
    }

    private void Awake() {
        Instance = this;
    }

    public void PlayerDead() {
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnOrDespawnPlayer(Dictionary<ulong, Transform> heads, ulong nId) {
        Debug.Log("CAlled and cameras size is " + heads.Count);
        OnSpawnOrDespawnPlayer?.Invoke(this, new SpawnPlayerEventArgs{heads = heads, networkIdObj = nId});
    }
}
