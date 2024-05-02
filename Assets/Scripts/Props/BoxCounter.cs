using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class BoxCounter : NetworkBehaviour
{
    public TextMeshProUGUI counterText;
    public Image counterIcon; 
    public NetworkVariable<bool> IsCup = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> IsPicture = new NetworkVariable<bool>(false);
    private NetworkVariable<FixedString64Bytes> selectedLayer = new NetworkVariable<FixedString64Bytes>();
    private NetworkVariable<int> count = new NetworkVariable<int>();
    private NetworkVariable<int> totalCount = new NetworkVariable<int>(3);
    public Door door;
    public Sprite spritePhone, spritePicture, spriteCup;

    private void OnEnable()
    {
        count.OnValueChanged += UpdateUI;
    }

    private void OnDisable()
    {
        count.OnValueChanged -= UpdateUI;
    }

    public override void OnNetworkSpawn()
    {
        
        UpdateUI(0, count.Value);
        SetIconForSelectedLayer();
        
    }
    void UpdateUI(int oldCount, int newCount)
    {
        counterText.text = newCount + "/" + totalCount.Value;
    }
   
    void SetIconForSelectedLayer()
    {
        if(IsCup.Value)
        {
            counterIcon.sprite = spriteCup;
            selectedLayer.Value = "Cup";
        } 
        else if (IsPicture.Value)
        {
            counterIcon.sprite = spritePicture;
            selectedLayer.Value = "Picture";
        } else
        {
            Debug.Log("La caja no sabe que contar porque no tiene valores asignados");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer && other.gameObject.layer == LayerMask.NameToLayer(selectedLayer.Value.ToString()))
        {
            count.Value += 1;
            if (count.Value == totalCount.Value)
            {
                door.OpenDoorRpc();
                GetComponent<PlaySFX>().Play(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsServer && other.gameObject.layer == LayerMask.NameToLayer(selectedLayer.Value.ToString()))
        {
            count.Value -= 1;
        }
    }

  


}
