using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyMovement enemyMovement;
    public GameObject attackCollider;
    [SerializeField] float attackCooldown = 1f;
    private float lastAttackTime;

    void Start()
    {
        attackCollider.SetActive(false);
        lastAttackTime = -attackCooldown;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            enemyMovement.enemyAnim.SetTrigger("isAttacking");
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer()
    {
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(attackCooldown);
        attackCollider.SetActive(false);
    }
}
