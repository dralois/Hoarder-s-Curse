using UnityEngine;
using System.Linq;
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
    // Strength potion duration
    private float _strengthPotionCurrentDuration;
    
    // Other stuff
    private Animator playerAnim;
    private EquippedUI currEquippedUI;
    private SpriteRenderer playerRenderer;

    [SerializeField]
    private List<InventoryItem> everythang;
    [SerializeField]
    private float _strengthPotionMaxDuration;

    // Prefab for bow projectile
    [SerializeField]
    private GameObject projectilePrefab;

    // Current equipment
    private InventoryItem.ItemType currentType = InventoryItem.ItemType.MeleeWeapon;
    private InventoryItem currentWeapon;
    private InventoryItem currentArmor;

    private void Start()
    {
        // Retrieve animator
        playerAnim = gameObject.GetComponentInChildren<Animator>();
        // Retrieve renderer
        playerRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        // Set the strength potion duration to 0
        _strengthPotionCurrentDuration = 0f;
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

        // On HealthPotion pressed
        if (Input.GetButtonDown("HealingPotion"))
        {
            LinkedList<InventoryItem> potions;

            if (InventoryManager.Instance.Inventory.TryGetValue(InventoryItem.ItemType.Potion, out potions))
            {
                // Get the First Potion
                LinkedListNode<InventoryItem> node = potions.First;
                while (node != null)
                {
                    if (((Potion)node.Value).potionType == Potion.PotionType.Healing)
                    {
                        Potion currPotion = (Potion)node.Value;
                        currPotion.isUsed = true;
                        currPotion.potionType = Potion.PotionType.Empty;
                        currPotion.sprite = currPotion.emptyPotion;

                        PlayerManager.Instance.HealingPotion();
                        break;
                    }
                    node = node.Next;
                }
            }
        }

        // On StrengthPotion pressed
        if (Input.GetButtonDown("StrengthPotion"))
        {
            LinkedList<InventoryItem> potions;

            if (InventoryManager.Instance.Inventory.TryGetValue(InventoryItem.ItemType.Potion, out potions))
            {
                // Get the First Potion
                LinkedListNode<InventoryItem> node = potions.First;
                while (node != null)
                {
                    if (((Potion)node.Value).potionType == Potion.PotionType.Strength)
                    {
                        Potion currPotion = (Potion)node.Value;
                        currPotion.isUsed = true;
                        currPotion.potionType = Potion.PotionType.Empty;
                        currPotion.sprite = currPotion.emptyPotion;

                        PlayerManager.Instance.SetBuff(true);
                        _strengthPotionCurrentDuration = _strengthPotionMaxDuration;
                        break;
                    }
                    node = node.Next;
                }
            }
        }

        // On Attack
        if (Input.GetButtonDown("Attack"))
        {
            if(currentWeapon != null)
            {
                // If melee weapon equiped
                if(currentWeapon.itemType == InventoryItem.ItemType.MeleeWeapon)
                {
                    // Cast to weapon
                    MeleeWeapon curr = (MeleeWeapon)currentWeapon;
                    // Play corresponding animation
                    switch (curr.meleeWeaponType)
                    {
                        case MeleeWeapon.MeleeWeaponType.Sword:
                            {
                                playerAnim.SetTrigger("AttackSword");
                                break;
                            }
                        case MeleeWeapon.MeleeWeaponType.Lance:
                            {
                                playerAnim.SetTrigger("AttackLance");
                                break;
                            }
                        default:
                            {
                                Debug.LogError("Invalid melee weapon type");
                                break;
                            }
                    }                    
                    // Raycast against enemies
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(playerRenderer.flipX ? -1 : 1, 0), 
                                                                curr.range, LayerMask.GetMask("Enemy"));
                    // Show range in debug
                    Debug.DrawLine(transform.position, transform.position + new Vector3(playerRenderer.flipX ? -1 : 1, 0) * curr.range, Color.white, .2f);
                    // Apply damage and knockback
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (!hit.collider.isTrigger)
                        {
                            // Knockback
                            hit.rigidbody.AddForceAtPosition((new Vector3(playerRenderer.flipX ? -1 : 1, 0)) * curr.damage, hit.point, ForceMode2D.Impulse);
                            // Damage
                            hit.transform.GetComponent<EnemyGround>().ApplyDamage(curr.damage * (int) PlayerManager.Instance.DmgAmp());
                        }
                    }
                }
                // Otherwise has to be ranged weapon
                else
                {
                    // Cast to weapon
                    RangedWeapon curr = (RangedWeapon)currentWeapon;
                    // Play corresponding animation
                    switch (curr.rangedWeaponType)
                    {
                        case RangedWeapon.RangedWeaponType.Bow:
                            {
                                playerAnim.SetTrigger("AttackBow");
                                break;
                            }
                        default:
                            {
                                Debug.LogError("Invalid ranged weapon type");
                                break;
                            }
                    }
                    // Fire projectile
                    GameObject newProj = Instantiate(projectilePrefab, transform);
                    newProj.GetComponent<PlayerProjectile>().damage = curr.damage;
                    newProj.GetComponent<Rigidbody2D>().velocity = new Vector3(playerRenderer.flipX ? -1 : 1, 0) * newProj.GetComponent<PlayerProjectile>().moveSpeed;
                }
            }
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

        // Keep track of the strength potion effect
        if (PlayerManager.Instance.isBuffed)
        {
            _strengthPotionCurrentDuration -= Time.fixedDeltaTime;
            if (_strengthPotionCurrentDuration <= 0f)
            {
                PlayerManager.Instance.SetBuff(false);
            }
        }
    }

    #region Inventory functions

    private EquippedUI equippedUI
    {
        get
        {
            if (currEquippedUI == null)
            {
                currEquippedUI = FindObjectOfType<EquippedUI>();
            }
            // Return UI
            return currEquippedUI;
        }
    }

    // Change current equipped weapon
    private void EquipWeapon(InventoryItem item)
    {
        // Select item
        item.selected = true;
        if (currentWeapon != null)
            currentWeapon.selected = false;
        // Set item
        currentWeapon = item;
        // Set stats
        switch (item.itemType)
        {
            case InventoryItem.ItemType.MeleeWeapon:
                {
                    item.selected = true;
                    MeleeWeapon curr = (MeleeWeapon)item;
                    currentType = InventoryItem.ItemType.MeleeWeapon;
                    if (equippedUI != null)
                        equippedUI.EquipWeapon(item);
                    break;
                }
            case InventoryItem.ItemType.RangedWeapon:
                {
                    RangedWeapon curr = (RangedWeapon)item;
                    currentType = InventoryItem.ItemType.RangedWeapon;
                    if (equippedUI != null)
                        equippedUI.EquipWeapon(item);
                    break;
                }
            default:
                {
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
                        if (curr.damageResistance > ((Armor)currentArmor).damageResistance)
                        {
                            item.selected = true;
                            currentArmor.selected = false;
                            // Set item
                            currentArmor = item;
                            // Apply armor
                            PlayerManager.Instance.setResistance(curr.damageResistance);
                            // Set UI
                            if (equippedUI != null)
                                equippedUI.EquipArmor(item);
                        }
                    }
                    else
                    {
                        item.selected = true;
                        // Set item
                        currentArmor = item;
                        // Apply armor
                        PlayerManager.Instance.setResistance(curr.damageResistance);
                        // Set UI
                        if (equippedUI != null)
                            equippedUI.EquipArmor(item);
                    }
                    break;
                }
            default:
                {
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
            if (currWeapons.Count > 0)
                EquipWeapon(currWeapons.First.Value);
        }
        else
        {
            // Switch back
            currentType = currentType == InventoryItem.ItemType.MeleeWeapon ?
                InventoryItem.ItemType.RangedWeapon : InventoryItem.ItemType.MeleeWeapon;
        }
    }

    #endregion
}