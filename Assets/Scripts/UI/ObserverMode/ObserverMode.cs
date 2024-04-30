using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObserverMode : MonoBehaviour
{
    
    public class SpawnPlayerEventArgs : EventArgs
    {
        public Dictionary<ulong, Transform> heads;
    }

    [SerializeField]
    private PlayerCamObsCardBtn playerCamObsCardBtnPrefab;
    private GameObject camerasObserverHolder;
    private Transform holderTransform;
    private Dictionary<ulong, Transform> heads;

    private void Start() {
        UIManager.Instance.OnPlayerDead += OnPlayerDead_Event;
        UIManager.Instance.OnSpawnOrDespawnPlayer += OnSpawnOrDespawnPlayer_Event;
        Hide();
    }

    private void OnPlayerDead_Event(object sender, EventArgs e)
    {
        Show();
        camerasObserverHolder = GameObject.FindGameObjectWithTag("PlayerObserverCard");
        holderTransform = camerasObserverHolder.transform;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnSpawnOrDespawnPlayer_Event(object sender, UIManager.SpawnPlayerEventArgs e) {
        if (camerasObserverHolder == null || holderTransform == null) { return; }
        
        Debug.Log("On spawn cameras size" + e.heads.Count);

        heads = e.heads;

        Debug.Log("Instantiating buttons");    
        foreach (var head in heads)
        {
            // if (head.Key == e.networkIdObj) continue;
            Debug.Log("heads id: " + head.Key);
            PlayerCamObsCardBtn selectButtonInstance = Instantiate(playerCamObsCardBtnPrefab, holderTransform);
            selectButtonInstance.SetCameraId(head.Key);
            selectButtonInstance.OnSwitchCamera += OnSwitchCamera;
            ulong key = head.Key;
            selectButtonInstance.btn.onClick.AddListener(() =>
            {
                selectButtonInstance.Click(key, e.networkIdObj);
            });
        }

    }


    public void OnSwitchCamera(object sender, PlayerCamObsCardBtn.PlayerCameraObsEventArgs e)
    {
        Debug.Log("HELLO!");
        Debug.Log("Swithing to camera " + e.cameraObjectId);
        PlayerMovement.Instance.SwitchHead(e.cameraObjectId, e.netid);
        
    }
}
