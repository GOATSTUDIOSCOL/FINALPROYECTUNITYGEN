using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public string description;
    public int quantity;
}
