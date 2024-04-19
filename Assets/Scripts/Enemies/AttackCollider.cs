using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public EnemyMovement enemyMovement;

    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Destroy(player.gameObject); // Destruye al jugador
            enemyMovement.OnPlayerKilled();
        }
    }
    
}
