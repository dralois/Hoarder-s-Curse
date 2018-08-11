using UnityEngine;

namespace Assets.Scripts
{
    public class MeleeWeapon : InventoryItem
    {
        public enum MeleeWeaponType : uint
        {
            Sword = 0,
            Lance = 1
        }

        public readonly MeleeWeaponType meleeWeaponType;

        public MeleeWeapon(MeleeWeaponType meleeWeaponType) : base(ItemType.MeleeWeapon)
        {
            this.meleeWeaponType = meleeWeaponType;
        }
    }
}
