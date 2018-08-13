using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryItems;
    [SerializeField]
    private GameObject backgroundItems;
    [SerializeField]
    private Color backgroundColor;
    [SerializeField]
    private Color selectedColor;

    private Image[] backgrounds;
    private Image[] images;

    void Start ()
    {
        InventoryManager.Instance._maxInventoryCount = (uint)inventoryItems.GetComponentsInChildren<Image>().Length;
        // Get images
        backgrounds = backgroundItems.GetComponentsInChildren<Image>();
        images = inventoryItems.GetComponentsInChildren<Image>();
    }
	
	void Update ()
    {
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
                    backgrounds[i].color = item.selected ? selectedColor : backgroundColor;
                    i++;
                }
            }
        }
	}
}