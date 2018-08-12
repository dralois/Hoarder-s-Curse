using UnityEngine;

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
        // On switch pickup pressed
        if (down)
        {
            PickupManager.Instance.switchActive();
        }
    }
}