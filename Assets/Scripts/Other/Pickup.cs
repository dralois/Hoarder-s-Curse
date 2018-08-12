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

    private void OnBecameInvisible()
    {
        Debug.Log("Im out boyz");
        // Destroy if camera can't see pickup anymore because it moved past
        if(Camera.main.transform.position.x > transform.position.x)
        {
            PickupManager.Instance.removeItem(this);
            Destroy(gameObject);
        }
    }

    public void setPickupable(bool enabled)
    {
        // En/disable UI desc
        descriptionUI.SetActive(enabled);
        // Enable additional button
        nextButton.SetActive(PickupManager.Instance.currTriggered > 1);
    }

    public void pickupItem()
    {
        // Add to inventory and destroy
        InventoryManager.Instance.AddItem(item);
        PickupManager.Instance.removeItem(this);
        Destroy(gameObject);
    }
}