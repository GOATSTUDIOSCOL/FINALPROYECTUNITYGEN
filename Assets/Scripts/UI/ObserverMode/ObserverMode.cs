using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObserverMode : MonoBehaviour
{
    
    public class SpawnPlayerEventArgs : EventArgs
    {
        public Dictionary<ulong, GameObject> cameras;
    }

    [SerializeField]
    private PlayerCamObsCardBtn playerCamObsCardBtnPrefab;
    private GameObject camerasObserverHolder;
    private Transform holderTransform;
    private Dictionary<ulong, GameObject> cameras;
    private int cameraPriority = 1;

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
        if (camerasObserverHolder == null || holderTransform == null) { return;
         }
        
        cameras = e.cameras;
        
        foreach (var cam in cameras)
        {
            PlayerCamObsCardBtn selectButtonInstance = Instantiate(playerCamObsCardBtnPrefab, holderTransform);
            selectButtonInstance.SetCameraId(cam.Key);
            selectButtonInstance.OnSwitchCamera += OnSwitchCamera;

            selectButtonInstance.btn.onClick.AddListener(() =>
            {
                selectButtonInstance.Click();
            });
        }

    }


    public void OnSwitchCamera(object sender, PlayerCamObsCardBtn.PlayerCameraObsEventArgs e)
    {
        // Debug.Log("Swithing to camera " + e.cameraObjectId);
        // if (cameras.ContainsKey(e.cameraObjectId)) {

        //     GameObject newCamera = cameras[e.cameraObjectId];
        //     newCamera.SetActive(true);
        //     cvc = newCamera.GetComponent<CinemachineVirtualCamera>();
        //     cvc.Priority = cameraPriority++;
        // }
    }
}
