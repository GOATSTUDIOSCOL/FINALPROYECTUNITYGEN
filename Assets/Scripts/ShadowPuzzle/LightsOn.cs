using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    [SerializeField]
    private GameObject Lamp;
    [SerializeField]
    private ObjectRotator skull;
    Collider currentPlayer;

    private void Awake()
    {
        Lamp.SetActive(false);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !skull.isOnPuzzle && !skull.puzzleSolved )
        {
            currentPlayer = collider;
            collider.GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>().enabled = false;
            skull.isOnPuzzle = true;
            Lamp.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider tmp)
    {
        if (tmp == currentPlayer)
        {
            ExitPuzzle();
        }
    }

    public void ExitPuzzle()
    {
        currentPlayer.GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>().enabled = true;
        skull.isOnPuzzle = false;
        Lamp.SetActive(false);
    }
}
