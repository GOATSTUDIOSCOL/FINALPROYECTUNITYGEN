using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Unity.Netcode;



public class PlayerDamage : NetworkBehaviour
{
    [SerializeField] float playerHealth = 3;
   // [SerializeField] float pushForce = 10f;
    public PostProcessingDamage postProcessingDamage;
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
        playerMovement = GetComponent<PlayerMovement>();
    }
  
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Damage"))
        {
           // UnityEngine.Vector3 hitDirection = (transform.position - other.transform.position).normalized;
           // transform.position += hitDirection * pushForce;

            switch(playerHealth)
            {
                case 3: 
                playerHealth -= 1;
               // postProcessingDamage.FirstDamageState();
                break;
                case 2: 
                playerHealth -= 1;
                //postProcessingDamage.UltimateDamageState();
                break;
                case 1: 
                playerHealth -= 1;
               // postProcessingDamage.DeadCameraState();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false; // Desactiva el script de movimiento
                }
                if(IsOwner)
                {
                    GameManager.instance.losePanel.SetActive(true);
                }
                
                break;
            }
        }
    }
}
