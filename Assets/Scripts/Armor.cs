using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Inventory/Armor", order = 5)]
    class Armor : InventoryItem
    {
        public enum ArmorType : uint
        {
            Default = 0,
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 3
        }

        public ArmorType armorType;
        public float damageResistance;
        public Armor() : base(ItemType.Armor) { }
    }
}
