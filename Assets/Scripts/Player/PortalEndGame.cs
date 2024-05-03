using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.winPanel.SetActive(true);
            GameManager.instance.EnableCursor();
        }
    }
}
