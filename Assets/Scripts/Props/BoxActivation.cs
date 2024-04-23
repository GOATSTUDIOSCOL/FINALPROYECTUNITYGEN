using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxActivation : MonoBehaviour
{
    [SerializeField] bool isOpen;
    [SerializeField] Animator boxAnim;
    // Update is called once per frame
    void Update()
    {
        if(isOpen)
        {
            boxAnim.SetBool("isOpen", true);
        } else
        {
            boxAnim.SetBool("isOpen", false);
        }
    }
}
