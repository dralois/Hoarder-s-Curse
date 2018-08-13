using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    public GameObject inventoryItems;    
    
	void Start ()
    {
        InventoryManager.Instance._maxInventoryCount = (uint)inventoryItems.GetComponentsInChildren<Image>().Length;        
	}
	
	void Update ()
    {
        Image[] images = inventoryItems.GetComponentsInChildren<Image>();
        int i = 0;
        // Iterate inventory and fill respective slots
		foreach(InventoryItem.ItemType itemType in Enum.GetValues(typeof(InventoryItem.ItemType)))
        {
            LinkedList<InventoryItem> items;

            if (InventoryManager.Instance.Inventory.TryGetValue(itemType, out items))
            {
                foreach (InventoryItem item in items)
                {
                    images[i].sprite = item.sprite;
                    i++;
                }
            }
        }
	}
}