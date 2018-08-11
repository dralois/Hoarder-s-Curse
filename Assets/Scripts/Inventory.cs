using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class Inventory : MonoBehaviour {

    [SerializeField]
    public Canvas InventoryUI;

    // Testing purposes
    [SerializeField]
    public MeleeWeapon weapon;

	// Use this for initialization
	void Start () {
        InventoryManager.Instance._maxInventoryCount = (uint)InventoryUI.GetComponentsInChildren<Image>().Length;

        // Testing purposes
        InventoryManager.Instance.AddItem(weapon);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Image[] images = InventoryUI.GetComponentsInChildren<Image>();
        int i = 0;

		foreach(InventoryItem.ItemType itemType in Enum.GetValues(typeof(InventoryItem.ItemType)))
        {
            List<InventoryItem> items;

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
