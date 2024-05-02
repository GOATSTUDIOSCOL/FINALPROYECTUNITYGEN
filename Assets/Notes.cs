using UnityEngine;

public class Notes : MonoBehaviour
{
    public GameObject notesLore;
    public bool actived = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && notesLore.activeSelf)
        {
            notesLore.SetActive(false);
        }
    }

    public void SetNoteState()
    {
        if (!notesLore.activeSelf)
        {
            notesLore.SetActive(true);
        }
    }

}
