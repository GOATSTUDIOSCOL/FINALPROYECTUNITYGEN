using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

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
    [SerializeField]
    private CinemachineVirtualCamera camera;

    private void Start() {
        heads = new Dictionary<ulong, Transform>();
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
        
        Transform headN = e.head;
        ulong nid = e.networkIdObj;
        bool op = e.op;

        if (op && heads.ContainsKey(nid)) {
            heads.Remove(nid);
        } else {
            heads.Add(nid, headN);
        }

        foreach(Transform trm in holderTransform.transform) {
             GameObject.Destroy(trm.gameObject);
        }

        Debug.Log("Instantiating buttons");    
        foreach (var head in heads)
        {
            // if (head.Key == nid) continue;
            Debug.Log("heads id: " + head.Key);
            PlayerCamObsCardBtn selectButtonInstance = Instantiate(playerCamObsCardBtnPrefab, holderTransform);
            selectButtonInstance.OnSwitchCamera += OnSwitchCamera;
            ulong key = head.Key;

            selectButtonInstance.SetCameraId(key);
            selectButtonInstance.btn.onClick.AddListener(() =>
            {
                selectButtonInstance.Click();
            });
        }

    }

    public void paintSwitchCameraBtn() {
        
    }


    public void OnSwitchCamera(object sender, PlayerCamObsCardBtn.PlayerCameraObsEventArgs e)
    {
        Debug.Log("HELLO!");
        Debug.Log("Swithing to camera from to " + e.cameraObjectId);
        Debug.Log("Heads " + heads.Count);
        Debug.Log("Contains " + heads.ContainsKey(e.cameraObjectId));
        if (heads.ContainsKey(e.cameraObjectId)) {
            // SwitchHeadServerRpc(NetworkObjectId, headObjectId);
            Transform newHead = heads[e.cameraObjectId];
           camera =  FindObjectOfType<CinemachineVirtualCamera>();
           camera.Follow = newHead;
        }
        // PlayerMovement.SwitchHead(e.cameraObjectId);
        // PlayerMovement.Instance.SwitchHead(e.cameraObjectId, e.netid);
        
    }
}
