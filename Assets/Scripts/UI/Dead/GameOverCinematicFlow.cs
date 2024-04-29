using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameOverCinematicFlow : MonoBehaviour
{
    public GameObject videoPlayerObject, gameOverPanel; 
    

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
    }
}

