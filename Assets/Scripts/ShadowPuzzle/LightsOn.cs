using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightsOn : MonoBehaviour
{
    [SerializeField]
    private GameObject Lamp;

    private void Awake()
    {
        Lamp.SetActive(false);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("JOIN COLLIDER");
            Lamp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider tmp)
    {
        Lamp.SetActive(false);
    }
}
