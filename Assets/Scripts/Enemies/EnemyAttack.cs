using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyMovement enemyMovement;
    public GameObject attackCollider;
    [SerializeField] float attackTime = 2f;
    void Start()
    {
        attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            enemyMovement.enemyAnim.SetTrigger("isAttacking");
            StartCoroutine(AttackTimer());
            
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(attackTime); 
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(attackTime); 
        attackCollider.SetActive(false);
    }
     
}
