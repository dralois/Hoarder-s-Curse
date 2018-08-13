using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {

    [SerializeField]
    private InventoryItem item;
    [SerializeField]
    private GameObject descriptionUI;
    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private Text descriptionText;

    // Turning sprite on the floor
    private SpriteRenderer itemRenderer;

    void Start()
    {
        // Retrieve sprite renderer for items
        itemRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        // Set sprite to render
        itemRenderer.sprite = item.sprite;
        // Depending on pickup type set description
        switch (item.itemType)
        {
            case InventoryItem.ItemType.Armor:
                {
                    Armor curr = (Armor)item;
                    descriptionText.text = curr.itemName.ToUpper() + "\nRESIST: " + curr.damageResistance.ToString();
                    break;
                }
            case InventoryItem.ItemType.Key:
                {
                    Key curr = (Key)item;
                    descriptionText.text = curr.itemName.ToUpper() + "\nLEVEL: " + curr.usableLevel.ToString();
                    break;
                }
            case InventoryItem.ItemType.MeleeWeapon:
                {
                    MeleeWeapon curr = (MeleeWeapon)item;
                    descriptionText.text = curr.itemName.ToUpper() + "\nDMG: " + curr.damage.ToString() + "\tRNG: " + curr.range.ToString();
                    break;
                }
            case InventoryItem.ItemType.Potion:
                {
                    Potion curr = (Potion)item;
                    descriptionText.text = curr.itemName.ToUpper() + "\nTYPE: " + curr.potionType.ToString().ToUpper();
                    break;
                }
            case InventoryItem.ItemType.RangedWeapon:
                {
                    RangedWeapon curr = (RangedWeapon)item;
                    descriptionText.text = curr.itemName.ToUpper() + "\nDMG: " + curr.damage.ToString() + "\tRNG: " + curr.range.ToString();
                    break;
                }
            case InventoryItem.ItemType.Useless:
                {
                    Useless curr = (Useless)item;
                    descriptionText.text = curr.itemName.ToUpper();
                    break;
                }
            default:
                {
                    descriptionText.text = "ERROR";
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enable pickup on player collision
        if (collision.tag == "Player")
        {
            PickupManager.Instance.addItem(this);
        }
        // Remove RB on ground collision
        else if(collision.tag == "Ground")
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Disable pickup on player collision exit
        if (collision.tag == "Player")
        {
            PickupManager.Instance.removeItem(this);
        }
    }

    public void SetItem(InventoryItem newItem)
    {
        // Set item
        item = newItem;
    }

    public void setPickupable(bool enabled)
    {
        // En/disable UI desc
        descriptionUI.SetActive(enabled);
        // Enable additional button
        nextButton.SetActive(PickupManager.Instance.currTriggered > 1);
        // Make item bigger to tell the difference
        itemRenderer.transform.localScale = new Vector3(enabled ? 2 : 1, enabled ? 2 : 1, 1);
    }

    public void pickupItem()
    {
        // Add to inventory and destroy
        if (InventoryManager.Instance.AddItem(item))
        {
            PickupManager.Instance.removeItem(this);
            Destroy(gameObject);
        }
        else
        {
            Camera.current.GetComponent<CameraMovement>().ApplyShake();
        }
    }
}