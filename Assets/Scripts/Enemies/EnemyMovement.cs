using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent aiNav;
    [SerializeField] PlayerMovement[] players;
    [SerializeField] PlayerMovement nearestPlayer;
    [SerializeField] float speed = 2f;

    
    //public Animator enemyAnim;
    public bool isDead=false;
    public float damageDistance = 5f;
    //[SerializeField] GameObject playerObj, camJumpscare;
    void Start()
    {
        aiNav.speed = speed;
        players = FindObjectsOfType<PlayerMovement>();
        InvokeRepeating("UpdateNearestPlayer", 0.5f, 2f); // Actualiza cada 0.5 segundos
    }

    private void Update() {
        aiNav.speed = speed;
    }
    void UpdateNearestPlayer()
    {
        if (players.Length == 0)
            return;

        float minDistance = float.MaxValue;
        PlayerMovement nearestPlayer = null;

        foreach (PlayerMovement player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null && aiNav != null && !isDead)
        {
            aiNav.SetDestination(nearestPlayer.transform.position);
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Destroy(player.gameObject); // Destruye al jugador
            StartCoroutine(UpdatePlayersAfterDelay());
        }
    }
     IEnumerator UpdatePlayersAfterDelay()
    {
        yield return new WaitForEndOfFrame(); // Espera hasta el final del frame para asegurar que el jugador esté completamente destruido
        UpdatePlayerList(); // Actualiza la lista de jugadores
        UpdateNearestPlayer(); // Busca un nuevo jugador más cercano
    }

    void UpdatePlayerList()
    {
        players = FindObjectsOfType<PlayerMovement>(); // Vuelve a encontrar todos los jugadores activos
    }
    
}

    // void Update()
    // {
    //     aiNav.destination = enemyDestination;
    //     actualSpeed = aiNav.speed;
    //     //HurtPlayer();
    // }
  
    // void HurtPlayer()
    // {   
    //     if(!isDead){
    //     float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
    //     if (distanceToPlayer <= damageDistance)
    //     {
    //         isDead = true;
            
    //         StartCoroutine("GamePlayerOver");
    //         playerObj.SetActive(false);
    //         camJumpscare.SetActive(true);
    //         enemyAnim.SetTrigger("jumpscare");
    //     }
    //     }
    // }

  
