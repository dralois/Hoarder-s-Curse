using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Key", menuName = "Inventory/Key", order = 2)]
    public class Key : InventoryItem
    {
        public uint usableLevel;

        public Key() : base(ItemType.Key) { }
    }
}
