using Cinemachine;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking.Types;

public class RpcTest : NetworkBehaviour
{
    public static RpcTest instance;

    private void Awake()
    {
        instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(0, NetworkObjectId);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    void TestClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        if (IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [Rpc(SendTo.Server)]
    void TestServerRpc(int value, ulong sourceNetworkObjectId)
    {
        //Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
        TestClientRpc(value, sourceNetworkObjectId);
    }

    [Rpc(SendTo.Server)]
    public void TestDespawnObjectRpc(ulong sourceNetworkObjectId)
    {
        NetworkObject[] networkObjects = FindObjectsOfType<NetworkObject>();

        // Loop through each network object to find the one with the matching network ID
        foreach (NetworkObject netObj in networkObjects)
        {
            // Check if the network ID of the current object matches the target network ID
            if (netObj.NetworkObjectId == sourceNetworkObjectId)
            {
                netObj.Despawn(true);
            }
        }
    }


    [Rpc(SendTo.Server)]
    public void TestParentObjectServerRpc(ulong PlayerId, ulong ObjectId)
    {
        NetworkObject[] networkObjects = FindObjectsOfType<NetworkObject>();
        NetworkObject playerFound = null;
        NetworkObject objectFound = null;
        // Loop through each network object to find the one with the matching network ID
        foreach (NetworkObject netObj in networkObjects)
        {
            // Check if the network ID of the current object matches the target network ID
            if (netObj.NetworkObjectId == ObjectId)
            {
                objectFound = netObj;
            }
            if (netObj.NetworkObjectId == PlayerId)
            {
                playerFound = netObj;
            }
        }
        if (objectFound != null && playerFound != null)
        {
           objectFound.transform.parent = playerFound.GetComponent<PlayerMovement>().playerCamera.transform;
        }
    }
}