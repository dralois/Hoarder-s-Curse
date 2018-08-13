using UnityEngine;

[CreateAssetMenu(fileName = "Useless", menuName = "Inventory/Useless", order = 6)]
public class Useless : InventoryItem
{
    public Useless() : base(ItemType.Useless) { }
}