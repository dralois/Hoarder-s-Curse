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
        if (Input.GetButtonDown("Pickup"))
        {
            // Add to inventory and destroy
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            descriptionUI.gameObject.SetActive(true);
            pickupable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
