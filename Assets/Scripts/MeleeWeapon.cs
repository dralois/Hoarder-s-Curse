using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Inventory/MeleeWeapon", order = 1)]
public class MeleeWeapon : InventoryItem
{
    public enum MeleeWeaponType : uint
    {
        Sword = 0,
        Lance = 1
    }

    public MeleeWeaponType meleeWeaponType;
    public int damage;

    public MeleeWeapon() : base(ItemType.MeleeWeapon) { }
}