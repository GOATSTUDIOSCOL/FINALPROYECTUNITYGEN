using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BoxActivation : NetworkBehaviour
{
    [SerializeField] bool isOpenLocal;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>();
    [SerializeField] Animator boxAnim;
    [SerializeField] AudioSource effect;
    [SerializeField] AudioClip sndfx;
    void Update()
    {
        if(isOpen.Value)
        {
            boxAnim.SetBool("isOpen", true);
            effect.PlayOneShot(sndfx);
        } else
        {
            boxAnim.SetBool("isOpen", false);
        }
    }
    [Rpc(SendTo.Server)]
    public void OpenBoxRpc()
    {
        isOpen.Value = true;
    }
}
