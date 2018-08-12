using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
    private InventoryItem item;
    [SerializeField]
    private GameObject descriptionUI;
    [SerializeField]
    private GameObject nextButton;

    // Turning sprite on the floor
    private SpriteRenderer itemRenderer;

    void Start()
    {
        // Retrieve sprite renderer for items
        itemRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        // Set sprite to render
        itemRenderer.sprite = item.sprite;
        
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
        InventoryManager.Instance.AddItem(item);
        PickupManager.Instance.removeItem(this);
        Destroy(gameObject);
    }
}