using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Inventory/MeleeWeapon", order = 3)]
    public class MeleeWeapon : InventoryItem
    {
        public enum MeleeWeaponType : uint
        {
            Sword = 0,
            Lance = 1
        }

        public MeleeWeaponType meleeWeaponType;
        public int damage;
        public float range;

        public MeleeWeapon() : base(ItemType.MeleeWeapon) { }
    }
}