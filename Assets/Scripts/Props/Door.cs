using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Door : NetworkBehaviour
{
    public Animator doorAnim;
    //public bool isOpen = false;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>();

    void Start()
    {
        isOpen.Value = false;
    }
    void Update()
    {
        if(isOpen.Value)
        {
            doorAnim.SetBool("isOpen", true);
        } else
        {
            doorAnim.SetBool("isOpen", false);
        }
    }

     [Rpc(SendTo.Server)]
    public void OpenDoorRpc()
    {
        Debug.Log("Se envia RPC");
        isOpen.Value = true;
    }
}
