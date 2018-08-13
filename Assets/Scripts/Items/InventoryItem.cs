using UnityEngine;

public abstract class InventoryItem : ScriptableObject {

    public enum ItemType : uint
    {
        Potion = 0,
        Key = 1,
        MeleeWeapon = 2,
        RangedWeapon = 3,
        Armor = 4,
        Useless = 5
    }

    public readonly ItemType itemType;
    public Sprite sprite;
    public string itemName;

    private static uint uid;

    protected InventoryItem(ItemType itemType)
    {
        this.itemType = itemType;
        uid++;
    }
}