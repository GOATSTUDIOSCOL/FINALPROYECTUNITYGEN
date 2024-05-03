using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;



public class PlayerDamage : NetworkBehaviour
{
    [SerializeField] float playerHealth = 3;
    public bool isAlive = true;
   // [SerializeField] float pushForce = 10f;
    private EnemyMovement enemyMovement;
     private PlayerMovement playerMovement;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false; 
            return;
        }
    }
    void Start() 
    {
        playerHealth = 3;
        isAlive = true;
        playerMovement = GetComponent<PlayerMovement>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
    }
  
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Damage"))
        {
           // UnityEngine.Vector3 hitDirection = (transform.position - other.transform.position).normalized;
           // transform.position += hitDirection * pushForce;

            switch(playerHealth)
            {
                case 3: 
                    if(IsOwner)
                    {
                        playerHealth -= 1;
                        PostProcessingDamage.instance.damage1.SetActive(true);
                    }
                break;
                case 2: 
                    if(IsOwner)
                    {
                        playerHealth -= 1;
                        PostProcessingDamage.instance.damage2.SetActive(true);
                    }
                break;
                case 1: 
                    playerHealth -= 1;
                    isAlive=false;
                    if (playerMovement != null)
                    {
                        playerMovement.enabled = false;
                        playerMovement.transform.position = new Vector3(-1969,5,318);

                    }
                    if(IsOwner)
                    {
                        other.GetComponentInParent<EnemyMovement>().OnPlayerKilled();
                        GameManager.instance.diePanel.SetActive(true);
                    }
                
                break;
            }
        }
    }
}
