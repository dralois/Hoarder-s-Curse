using UnityEngine;

public class InventoryItem : ScriptableObject {

    public enum ItemType : uint
    {
        Potion = 0,
        Key = 1,
        MeleeWeapon = 2,
        RangedWeapon = 3,
        Armor = 4
    }

    public readonly ItemType itemType;

}