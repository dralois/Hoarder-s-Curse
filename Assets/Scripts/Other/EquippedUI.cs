using UnityEngine.UI;
using UnityEngine;

public class EquippedUI : MonoBehaviour {

    // UI stuff
    [SerializeField]
    private Image currentWeaponImage;
    [SerializeField]
    private Text currentWeaponText;
    [SerializeField]
    private Image currentArmorImage;
    [SerializeField]
    private Text currentArmorText;
    
    // Equips an armor
    public void EquipArmor(InventoryItem item)
    {
        switch (item.itemType)
        {
            case InventoryItem.ItemType.Armor:
                {
                    // Set Sprite
                    currentArmorImage.sprite = item.sprite;
                    // Change text
                    currentArmorText.text = "RESIST:\n" + (Mathf.Round(((Armor)item).damageResistance * 100f)).ToString() + "%";
                    break;
                }
            default:
                {
                    // Shouldnt happen
                    currentWeaponText.text = "ERROR";
                    break;
                }
            }
        }

    // Equips a weapon
    public void EquipWeapon(InventoryItem item)
    {
        switch (item.itemType)
        {
            case InventoryItem.ItemType.RangedWeapon:
                {
                    // Set Sprite
                    currentWeaponImage.sprite = item.sprite;
                    // Change text
                    currentWeaponText.text = "DMG: " + ((RangedWeapon) item).damage.ToString() + "\nRNG: " + ((RangedWeapon)item).range.ToString();
                    break;
                }
            case InventoryItem.ItemType.MeleeWeapon:
                {
                    // Set Sprite
                    currentWeaponImage.sprite = item.sprite;
                    // Change text
                    currentWeaponText.text = "DMG: " + ((MeleeWeapon)item).damage.ToString() + "\nRNG: " + ((MeleeWeapon)item).range.ToString();
                    break;
                }
            default:
                {
                    // Shouldnt happen
                    currentWeaponText.text = "ERROR";                        
                    break;
                }
        }
    }
}