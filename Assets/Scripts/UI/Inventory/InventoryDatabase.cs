using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Inventory Database", menuName = "Inventory/Database")]
public class InventoryDatabase : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();
}
