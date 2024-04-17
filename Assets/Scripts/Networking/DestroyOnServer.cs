using UnityEngine;
using System;
using Unity.Netcode;

public class DestroyOnServer : NetworkBehaviour
{
    [ServerRpc]
    void DestroyObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        if (IsServer)
        {
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    void DestroyObjectClientRpc(ClientRpcParams rpcParams = default)
    {
        if (!IsServer)
        {
            Destroy(gameObject);
        }
    }


    public void DestroyObject()
    {
        if (IsServer)
        {
            DestroyObjectServerRpc();
        }
        else
        {
            DestroyObjectClientRpc();
        }
    }
}
