using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Notes : MonoBehaviour
{
    public GameObject notesLore;
    public bool actived;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E ) && actived == true)
        {
            notesLore.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && actived == true)
        {
            notesLore.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            actived = true;
        }
    }
}
