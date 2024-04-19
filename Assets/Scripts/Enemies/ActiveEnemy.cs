using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemy : MonoBehaviour
{
    public GameObject enemy;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            enemy.SetActive(true);
        }
    }
}
