using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnim;
    public bool isOpen = false;
    void Start()
    {
        isOpen = false;
    }

    void Update()
    {
        if(isOpen)
        {
            doorAnim.SetBool("isOpen", true);
        } else
        {
            doorAnim.SetBool("isOpen", false);
        }
    }
}
