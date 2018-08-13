using UnityEngine;

public class Loader : MonoBehaviour {

    // Managers
    [SerializeField]
    private GameObject inventoryManager;
    [SerializeField]
    private GameObject playerManager;
    [SerializeField]
    private GameObject pickupManager;
    [SerializeField]
    private GameObject levelManager;

    // Load managers on game start
    void Awake()
    {
        if (InventoryManager.Instance == null)
            Instantiate(inventoryManager);
        if (PlayerManager.Instance == null)
            Instantiate(playerManager);
        if (PickupManager.Instance == null)
            Instantiate(pickupManager);
        if (LevelManager.Instance == null)
            Instantiate(levelManager);
    }
}