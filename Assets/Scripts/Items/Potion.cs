using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Inventory/Potion", order = 1)]
    public class Potion : InventoryItem
    {
        public enum PotionType : uint
        {
            Healing = 0,
            Strength = 1,
            Empty
        }

        public PotionType potionType;
        public Potion() : base(ItemType.Potion) { }
    }
}
