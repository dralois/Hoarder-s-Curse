using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    // D-pad directions
    private bool up;
    private bool down;
    private bool right;
    private bool left;
    // Last direction
    private int lastDpadX;
    private int lastDpadY;
    
    // Other stuff
    private Animator playerAnim;
    [SerializeField]
    private List<InventoryItem> everythang;

    // UI stuff
    [SerializeField]
    private Image currentWeaponImage;
    [SerializeField]
    private Text currentWeaponText;
    [SerializeField]
    private Image currentArmorImage;
    [SerializeField]
    private Text currentArmorText;

    // Current equipment
    private InventoryItem.ItemType currentType = InventoryItem.ItemType.MeleeWeapon;
    private InventoryItem currentWeapon;
    private InventoryItem currentArmor;

    // Change current equipped weapon
    private void EquipWeapon(InventoryItem item)
    {
        // Select item
        item.selected = true;
        if (currentWeapon != null)
            currentWeapon.selected = false;
        // Set item
        currentWeapon = item;
        // Set Sprite
        currentWeaponImage.sprite = item.sprite;
        // Set stats
        switch (item.itemType)
        {
            case InventoryItem.ItemType.MeleeWeapon:
                {
                    item.selected = true;
                    MeleeWeapon curr = (MeleeWeapon)item;
                    currentType = InventoryItem.ItemType.MeleeWeapon;
                    currentWeaponText.text = "DMG: " + curr.damage.ToString() + "\nRNG: " + curr.range.ToString();
                    break;
                }
            case InventoryItem.ItemType.RangedWeapon:
                {
                    RangedWeapon curr = (RangedWeapon)item;
                    currentType = InventoryItem.ItemType.RangedWeapon;
                    currentWeaponText.text = "DMG: " + curr.damage.ToString() + "\nRNG: " + curr.range.ToString();
                    break;
                }
            default:
                {
                    currentWeaponText.text = "ERROR";
                    break;
                }
        }
    }

    // Change current equipped armor
    private void EquipArmor(InventoryItem item)
    {
        // Set stats
        switch (item.itemType)
        {
            case InventoryItem.ItemType.Armor:
                {
                    // Cast to armor
                    Armor curr = (Armor)item;
                    // If we have an armor
                    if (currentArmor != null)
                    {
                        // If damage resitance is higher then equip
                        if(curr.damageResistance > ((Armor)currentArmor).damageResistance)
                        {
                            item.selected = true;
                            currentArmor.selected = false;
                            // Set item
                            currentArmor = item;
                            // Set Sprite
                            currentArmorImage.sprite = item.sprite;
                            // Change text
                            currentArmorText.text = "RESIST:\n" + (Mathf.Round(curr.damageResistance * 100f)).ToString() + "%";
                        }
                    }
                    else
                    {
                        item.selected = true;
                        // Set item
                        currentArmor = item;
                        // Set Sprite
                        currentArmorImage.sprite = item.sprite;
                        // Change text
                        currentArmorText.text = "RESIST:\n" + (Mathf.Round(curr.damageResistance * 100f)).ToString() + "%";
                    }
                    break;
                }
            default:
                {
                    currentWeaponText.text = "ERROR";
                    break;
                }
        }
    }

    // Change weapon type
    private void changeWeaponType()
    {
        // Change type
        currentType = currentType == InventoryItem.ItemType.MeleeWeapon ? 
            InventoryItem.ItemType.RangedWeapon : InventoryItem.ItemType.MeleeWeapon;
        // Get current inventory
        LinkedList<InventoryItem> currWeapons;
        // Select the first available one if there is one or dont change type
        if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
        {
            if(currWeapons.Count > 0)
                EquipWeapon(currWeapons.First.Value);
        }
        else
        {
            // Switch back
            currentType = currentType == InventoryItem.ItemType.MeleeWeapon ? 
                InventoryItem.ItemType.RangedWeapon : InventoryItem.ItemType.MeleeWeapon;
        }        
    }

    private void Start()
    {
        // Retrieve animator
        playerAnim = gameObject.GetComponentInChildren<Animator>();
    }

    void Update ()
    {
        // Get current dpad presses
        if (Input.GetAxis("DPadX") == 1 && lastDpadX != 1) { right = true; } else { right = false; }
        if (Input.GetAxis("DPadX") == -1 && lastDpadX != -1) { left = true; } else { left = false; }
        if (Input.GetAxis("DPadY") == 1 && lastDpadY != 1) { up = true; } else { up = false; }
        if (Input.GetAxis("DPadY") == -1 && lastDpadY != -1) { down = true; } else { down = false; }
        // Save last direction
        lastDpadX = (int) Input.GetAxis("DPadX");
        lastDpadY = (int) Input.GetAxis("DPadY");

        // On pickup pressed
        if (Input.GetButtonDown("Pickup"))
        {
            // If successful pickup
            if (PickupManager.Instance.pickupActive())
            {
                // Equip item if it is equipable
                switch(InventoryManager.Instance.LastPickup.itemType)
                {
                    case InventoryItem.ItemType.MeleeWeapon:
                    {
                        EquipWeapon(InventoryManager.Instance.LastPickup);
                        break;
                    }
                    case InventoryItem.ItemType.RangedWeapon:
                    {
                        EquipWeapon(InventoryManager.Instance.LastPickup);
                        break;
                    }
                    case InventoryItem.ItemType.Armor:
                    {
                        EquipArmor(InventoryManager.Instance.LastPickup);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }

        // On Attack
        if (Input.GetButtonDown("Attack"))
        {
            playerAnim.SetTrigger("Attack");
        }

        // On switch pickup pressed
        if (down)
        {
            PickupManager.Instance.switchActive();
        }

        // Debug drop item / switch weapon type
        if (up)
        {
            PickupManager.Instance.SpawnItem(transform.position, everythang);
            // Change type
            changeWeaponType();
        }

        // Switch active weapon right
        if (right)
        {
            // If we have a weapon
            if(currentWeapon != null)
            {
                LinkedList<InventoryItem> currWeapons;
                // Try to get the current weapon list
                if(InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    // Find the weapon in the inventory
                    LinkedListNode<InventoryItem> node = currWeapons.Find(currentWeapon);
                    // Select the next weapon (or first again)
                    EquipWeapon(node.Next == null ? currWeapons.First.Value : node.Next.Value);
                }
            }
            else
            {
                LinkedList<InventoryItem> currWeapons;
                // Try to get the current weapon list
                if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    // Select the last weapon if possible
                    if(currWeapons.Count > 0)
                        EquipWeapon(currWeapons.Last.Value);
                }
            }
        }

        // Switch active weapon left
        if (left)
        {
            // If we have a weapon
            if (currentWeapon != null)
            {
                LinkedList<InventoryItem> currWeapons;
                // Try to get the current weapon list
                if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    // Find the weapon in the inventory
                    LinkedListNode<InventoryItem> node = currWeapons.Find(currentWeapon);
                    // Select the next weapon (or last again)
                    EquipWeapon(node.Previous == null ? currWeapons.Last.Value : node.Previous.Value);
                }
            }
            else
            {
                LinkedList<InventoryItem> currWeapons;
                // Try to get the current weapon list
                if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    // Select the first weapon if possible
                    if (currWeapons.Count > 0)
                        EquipWeapon(currWeapons.First.Value);
                }
            }
        }
    }    
}