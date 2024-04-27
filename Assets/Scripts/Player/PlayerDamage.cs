using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class PlayerDamage : MonoBehaviour
{
    [SerializeField] float playerHealth = 3;
   // [SerializeField] float pushForce = 10f;
    public PostProcessingDamage postProcessingDamage;
   void Start() {
    playerHealth = 3;
   }
    // Update is called once per frame
    void Update()
    {
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
                postProcessingDamage.FirstDamageState();
                break;
                case 2: 
                playerHealth -= 1;
                postProcessingDamage.UltimateDamageState();
                break;
                case 1: 
                playerHealth -= 1;
                postProcessingDamage.DeadCameraState();
                break;
            }
        }
    }
}
