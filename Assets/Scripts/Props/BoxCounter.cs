using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterIcon; // Asume que tienes un Image UI para el ícono

    private string[] layers = {"Phone", "Picture", "Cup"};
    private string selectedLayer;
    [SerializeField]private int count = 0;
    [SerializeField]private int totalCount;
    public Door door;

    public Sprite spritePhone, spritePicture, spriteCup;

    void Start()
    {
        selectedLayer = layers[UnityEngine.Random.Range(0, layers.Length)];
        int layerIndex = LayerMask.NameToLayer(selectedLayer);
        totalCount = FindObjectsOfType<GameObject>().Count(g => g.layer == layerIndex);
        
        UpdateUI(0);
        SetIconForSelectedLayer(); 
    }



    void UpdateUI(int currentCount)
    {
        counterText.text = currentCount + "/" + totalCount;
    }

    void SetIconForSelectedLayer()
    {
         switch (selectedLayer)
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
        if (other.gameObject.layer == LayerMask.NameToLayer(selectedLayer))
        {
            count += 1;
            UpdateUI(count);
            if(count == totalCount)
            {
            door.isOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(selectedLayer))
        {
            count -= 1;
            UpdateUI(count);
        }
    }

    
}
