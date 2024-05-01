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
        public Transform head;
        public ulong networkIdObj;
        public bool op;
    }

    private void Awake() {
        Instance = this;
    }

    public void PlayerDead() {
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnOrDespawnPlayer(Transform head, ulong nId, bool op) {
        Debug.Log("Called in UIManager and networkId is " + nId);
        OnSpawnOrDespawnPlayer?.Invoke(this, new SpawnPlayerEventArgs{head = head, networkIdObj = nId, op = op});
    }
}
