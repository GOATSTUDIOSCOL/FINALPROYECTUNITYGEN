using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BoxActivation : NetworkBehaviour
{
    [SerializeField] bool isOpenLocal;
    public GameObject noCardPanel;
    private NetworkVariable<bool> isOpen = new NetworkVariable<bool>();
    [SerializeField] Animator boxAnim;

    private void Start()
    {
        noCardPanel.SetActive(false);
    }
    void Update()
    {
        if (isOpen.Value)
        {
            boxAnim.SetBool("isOpen", true);
        }
        else
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

    public void NoCardRpc()
    {
        StartCoroutine(NoCardCoroutine());
    }
    IEnumerator NoCardCoroutine()
    {
        GetComponent<PlaySFX>().Play(1);
        noCardPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        noCardPanel.SetActive(false);

    }
}
