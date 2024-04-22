using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterIcon; // Asume que tienes un Image UI para el ícono

    private string[] tags = {"Phone", "Picture", "Cup"};
    private string selectedTag;
    [SerializeField]private int count = 0;
    [SerializeField]private int totalCount;

    public Sprite spritePhone, spritePicture, spriteCup;

    void Start()
    {
        selectedTag = tags[UnityEngine.Random.Range(0, tags.Length)];
        totalCount = GameObject.FindGameObjectsWithTag(selectedTag).Length;
        
        UpdateUI(0);
        SetIconForSelectedTag(); // Asegúrate de implementar este método para actualizar el ícono
    }

    void UpdateUI(int currentCount)
    {
        counterText.text = currentCount + "/" + totalCount;
    }

    void Update() {
        if(count == totalCount)
        {

        }
    }

    void SetIconForSelectedTag()
    {
         switch (selectedTag)
        {
            case "Phone":
                counterIcon.sprite = spritePhone;
                break;
            case "Picture":
                counterIcon.sprite = spritePicture;
                break;
            case "Cup":
                counterIcon.sprite = spriteCup;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(selectedTag))
        {
            count += 1;
            UpdateUI(count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(selectedTag))
        {
            count -= 1;
            UpdateUI(count);
        }
    }

    
}
