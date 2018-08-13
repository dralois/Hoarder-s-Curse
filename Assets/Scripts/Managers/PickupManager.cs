using UnityEngine;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    [SerializeField]
    private GameObject pickupPrefab;

    // Singleton instance
    private static PickupManager _instance = null;

    // Current number of triggered items
    public int currTriggered { get; private set; }
    private LinkedList<Pickup> currItems;
    private LinkedListNode<Pickup> current = null;    

    #region Spawning    

    public void SpawnItem(Vector2 at, List<InventoryItem> spawnList)
    {
        // Select random number in range
        int toSpawn = Random.Range(0, spawnList.Count - 1);
        // Spawn a pickup
        GameObject spawned = Instantiate(pickupPrefab, at, Quaternion.identity);
        // Set the item
        spawned.GetComponent<Pickup>().SetItem(Instantiate(spawnList[toSpawn]));
    }

    #endregion

    #region On Ground

    public void addItem(Pickup item)
    {
        if (!currItems.Contains(item))
        {
            // Select and add
            currTriggered++;
            current = currItems.AddLast(item);
            switchActive();
        }
    }

    public void removeItem(Pickup item)
    {
        if (currItems.Contains(item))
        {
            currTriggered--;
            // Disable UI or switch active
            if (current == currItems.Find(item))
                if(currTriggered > 0)
                    switchActive();
            else
                current.Value.setPickupable(false);
            // Remove
            currItems.Remove(item);
        }
    }
    
    public void switchActive()
    {    
        // If items are in the list
        if(currTriggered > 0)
        {
            // Switch through items
            current.Value.setPickupable(false);
            current = current.Next == null ? currItems.First : current.Next;
            // Enable pickup
            current.Value.setPickupable(true);          
        }
    }

    public bool pickupActive()
    {
        // If items available pick it up
        if(currTriggered > 0)
        {
            bool worked = current.Value.pickupItem();
            switchActive();
            return worked;
        }
        // Not successful
        return false;
    }

    #endregion

    #region Singleton

    public static PickupManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
            Destroy(gameObject);

        // Make sure it persits
        DontDestroyOnLoad(gameObject);
        // Create pickup list
        currItems = new LinkedList<Pickup>();
        // Seed random generator
        Random.InitState(System.DateTime.Now.Millisecond * System.DateTime.Now.Second);
    }

    #endregion
}