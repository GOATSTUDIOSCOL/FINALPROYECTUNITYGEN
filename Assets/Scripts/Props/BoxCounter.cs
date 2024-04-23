using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BoxCounter : NetworkBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterIcon; // Asume que tienes un Image UI para el ícono

    private string[] layers = {"Phone", "Picture", "Cup"};
    private string selectedLayer;
    [SerializeField]private int count = 0;
    [SerializeField]private int totalCount = 1;
    public Door door;

    public Sprite spritePhone, spritePicture, spriteCup;

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            enabled=false;
            return;
        }
        selectedLayer = layers[UnityEngine.Random.Range(0, layers.Length)];
        int layerIndex = LayerMask.NameToLayer(selectedLayer);
        totalCount = FindObjectsOfType<GameObject>().Count(g => g.layer == layerIndex);
        
        UpdateUI(0);
        SetIconForSelectedLayer(); 
    }
    void Start()
    {
        
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
            door.OpenDoorRpc();
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
