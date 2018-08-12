using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "RangedWeapon", menuName = "Inventory/RangedWeapon", order = 4)]
    public class RangedWeapon : InventoryItem
    {
        public enum RangedWeaponType : uint
        {
            Crossbow = 0,
            Bow = 1
        }

        public RangedWeaponType rangedWeaponType;
        public int damage;
        public float range;
        public RangedWeapon() : base(ItemType.RangedWeapon) { }
    }
}
