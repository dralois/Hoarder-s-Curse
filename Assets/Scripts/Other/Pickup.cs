using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
    private InventoryItem item;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Canvas descriptionUI;

    // Turning sprite on the floor
    SpriteRenderer itemRenderer;
    bool pickupable = false;

	void Start () {
        // Retrieve sprite renderer for items
        itemRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Set sprite to render
        itemRenderer.sprite = item.sprite;
        // Disable flavor text
        descriptionUI.gameObject.SetActive(false);
	}
	
	void Update () {
        // Spin it around the Y axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        // On pickup
        if (Input.GetButtonDown("Pickup") && pickupable)
        {
            // Add to inventory and destroy
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enable pickup on player collision
        if (collision.tag == "Player")
        {
            descriptionUI.gameObject.SetActive(true);
            pickupable = true;
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
            descriptionUI.gameObject.SetActive(false);
            pickupable = false;
        }
    }

    private void LateUpdate()
    {
        // UI should not spin
        descriptionUI.transform.rotation = Quaternion.identity;
    }
}
