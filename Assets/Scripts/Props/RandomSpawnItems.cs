using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
public class RandomSpawnItems : NetworkBehaviour
{
    [SerializeField] private GameObject[] randomPoints;
    [SerializeField] private GameObject[] pickableObjects; // Tipos de power up
    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            enabled=false;
            return;
        }
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;

        SpawnObjectRpc();
    }

    public void SpawnObjectRpc()
    {
        List<int> availablePoints = new List<int>();
        for (int i = 0; i < randomPoints.Length; i++)
        {
            availablePoints.Add(i);
        }

        for(int i = 0; i < pickableObjects.Length; i++)
        {
            if (availablePoints.Count == 0)
            {
                break; // No hay más puntos disponibles para colocar objetos
            }

            int pointIndex = Random.Range(0,availablePoints.Count); 
            int selectedPoint = availablePoints[pointIndex]; 
           
            Vector3 newPointPosition = randomPoints[selectedPoint].transform.position; 
            pickableObjects[i].transform.position = newPointPosition; 
        
            availablePoints.RemoveAt(pointIndex);
        }
    }

    void ClientConnected(ulong u)
    {
       
    }
    async void ClientDisconnected(ulong u)
    {
        await Task.Yield();
        
    }
    
    
}
