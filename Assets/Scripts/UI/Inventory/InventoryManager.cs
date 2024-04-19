using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryDatabase inventoryDatabase;
    public Image[] slots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            CleanItems();
        }
    }

    private void Start()
    {
        ShowInGame();
    }

    public void CleanItems()
    {
        foreach (var item in inventoryDatabase.items) 
        {
            item.quantity = 0;
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        if (inventoryDatabase.items.Contains(item))
        {
            if (item.itemName == "Key")
            {
                GameManager.instance.AddKeyRpc();
            }
            item.quantity++;
        }
        else
        {
            InventoryItem newItem = Instantiate(item);
            newItem.quantity = 1;
            inventoryDatabase.items.Add(newItem);
        }
        ShowInGame();
    }

    public void RemoveItemFromInventory(InventoryItem item)
    {
        if (inventoryDatabase.items.Contains(item))
        {
            if (item.quantity > 1)
            {
                item.quantity--;
            }
            else
            {
                inventoryDatabase.items.Remove(item);
            }
        }
        ShowInGame();
    }

    public void ShowInGame()
    {
        if (inventoryDatabase != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventoryDatabase.items.Count)
                {
                    InventoryItem item = inventoryDatabase.items[i];
                    slots[i].sprite = item.itemIcon;
                    slots[i].gameObject.SetActive(true);
                    TextMeshProUGUI quantityText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (quantityText != null)
                    {
                        quantityText.text = item.quantity.ToString();
                    }
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
