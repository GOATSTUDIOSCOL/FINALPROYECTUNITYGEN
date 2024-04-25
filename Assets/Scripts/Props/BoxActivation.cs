using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BoxActivation : NetworkBehaviour
{
    [SerializeField] bool isOpenLocal;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>();
    [SerializeField] Animator boxAnim;
    void Update()
    {
        if(isOpen.Value)
        {
            boxAnim.SetBool("isOpen", true);
        } else
        {
            boxAnim.SetBool("isOpen", false);
        }
    }
    [Rpc(SendTo.Server)]
    public void OpenBoxRpc()
    {
        isOpen.Value = true;
        GetComponent<PlaySFX>().Play(0);
    }
}
