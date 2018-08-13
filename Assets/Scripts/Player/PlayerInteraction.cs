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
    private void setWeapon(InventoryItem item)
    {
        // Set item
        currentWeapon = item;
        // Set Sprite
        currentWeaponImage.sprite = item.sprite;
        // Set stats
        switch (item.itemType)
        {
            case InventoryItem.ItemType.MeleeWeapon:
            {
                MeleeWeapon curr = (MeleeWeapon)item;
                currentWeaponText.text = "DMG: " + curr.damage.ToString() + "\nRNG: " + curr.range.ToString();
                break;
            }
            case InventoryItem.ItemType.RangedWeapon:
            {
                RangedWeapon curr = (RangedWeapon)item;
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
    private void setArmor(InventoryItem item)
    {
        // Set item
        currentArmor = item;
        // Set Sprite
        currentArmorImage.sprite = item.sprite;
        // Set stats
        switch (item.itemType)
        {
            case InventoryItem.ItemType.Armor:
            {
                Armor curr = (Armor)item;
                currentWeaponText.text = "RESIST:\n" + (Mathf.Round(curr.damageResistance * 100f)).ToString() + "%";
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
                setWeapon(currWeapons.First.Value);
        }
        else
        {
            // Switch back
            currentType = currentType == InventoryItem.ItemType.MeleeWeapon ? 
                InventoryItem.ItemType.RangedWeapon : InventoryItem.ItemType.MeleeWeapon;
            // Also set weapon if none was set
            if(currentWeapon == null)
            {
                if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    if (currWeapons.Count > 0)
                        setWeapon(currWeapons.First.Value);
                }
            }
        }

        Debug.Log(currentType);
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
            PickupManager.Instance.pickupActive();
            // Try set inital weapon
            if (currentWeapon == null)
            {
                // Get current inventory
                LinkedList<InventoryItem> currWeapons;
                // Try selected type first
                if (InventoryManager.Instance.Inventory.TryGetValue(currentType, out currWeapons))
                {
                    if(currWeapons.Count > 0)
                        currentWeapon = currWeapons.First.Value;
                }
                else
                {
                    // Attempt other type
                    changeWeaponType();
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
                    setWeapon(node.Next == null ? node.Value : node.Next.Value);
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
                        setWeapon(currWeapons.Last.Value);
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
                    // Select the next weapon (or first again)
                    setWeapon(node.Previous == null ? node.Value : node.Previous.Value);
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
                        setWeapon(currWeapons.First.Value);
                }
            }
        }
    }    
}