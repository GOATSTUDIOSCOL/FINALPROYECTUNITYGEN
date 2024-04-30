using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Unity.Netcode;

public class GameOverCinematicFlow : NetworkBehaviour
{
    public GameObject videoPlayerObject, gameOverPanel;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        gameOverPanel.SetActive(false);
        videoPlayerObject.SetActive(true);
        StartCoroutine(WaitForVideoEnd());
    }

    IEnumerator WaitForVideoEnd()
    {
        yield return new WaitForSeconds(8f);
        videoPlayerObject.SetActive(false);
        gameOverPanel.SetActive(true);
        GameManager.instance.EnableCursor();
    }
}

