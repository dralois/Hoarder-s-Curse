using UnityEngine;

public class Loader : MonoBehaviour {

    // Managers
    [SerializeField]
    private GameObject inventoryManager;

    // Load managers on game start
    void Awake()
    {
        if (InventoryManager.Instance == null)
            Instantiate(inventoryManager);
    }
}