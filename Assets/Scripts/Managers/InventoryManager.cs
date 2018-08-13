using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    public uint _maxInventoryCount;

    private static InventoryManager _instance = null;

    private Dictionary<InventoryItem.ItemType, LinkedList<InventoryItem>> _inventory = null;
    private InventoryItem _lastPickup;
    private uint _totalInventoryCount = 0;

    public static InventoryManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Returns current inventory
    public Dictionary<InventoryItem.ItemType, LinkedList<InventoryItem>> Inventory
    {
        get
        {
            return _inventory;
        }
    }

    public InventoryItem LastPickup
    {
        get
        {
            return _lastPickup;
        }
    }

    #region Add/Remove

    // Add item if inventory not full
    public bool AddItem(InventoryItem item)
    {
        LinkedList<InventoryItem> items;

        if (!_inventory.TryGetValue(item.itemType, out items))
        {
            _inventory.Add(item.itemType, new LinkedList<InventoryItem>());            
        }

        if (_totalInventoryCount < _maxInventoryCount)
        {
            _inventory[item.itemType].AddLast(item);
            _totalInventoryCount++;
            _lastPickup = item;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    // Remove item if in inventory
    public bool RemoveItem(InventoryItem item)
    {
        LinkedList<InventoryItem> items;

        if (!_inventory.TryGetValue(item.itemType, out items))
        {
            _inventory.Add(item.itemType, new LinkedList<InventoryItem>());
        }

        if (_inventory[item.itemType].Contains(item))
        {
            _inventory[item.itemType].Remove(item);
            if (_lastPickup == item)
                _lastPickup = null;
            _totalInventoryCount--;
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Singleton

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }        
        else if (_instance != this)
            Destroy(gameObject);

        // Make sure it persits
        DontDestroyOnLoad(gameObject);
        // Create inventory object
        _inventory = new Dictionary<InventoryItem.ItemType, LinkedList<InventoryItem>>();
    }

    #endregion
}