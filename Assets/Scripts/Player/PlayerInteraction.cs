using UnityEngine;
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
        if (up)
        {
            PickupManager.Instance.SpawnItem(transform.position, everythang);
        }
    }    
}