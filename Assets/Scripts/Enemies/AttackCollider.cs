using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public EnemyMovement enemyMovement;

    private void OnTriggerStay(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.gameObject.SetActive(false);
            enemyMovement.OnPlayerKilled();
        }
    }
    
}
