using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Inventory/Potion", order = 1)]
public class Potion : InventoryItem
{
    public enum PotionType : uint
    {
        Healing = 0,
        Strength = 1,
        Empty = 2
    }

    [SerializeField]
    public bool isUsed;
    [SerializeField]
    public Sprite emptyPotion;

    public PotionType potionType;
    public Potion() : base(ItemType.Potion) { }
}