using UnityEngine;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    // Singleton instance
    private static PickupManager _instance = null;

    // Current number of triggered items
    public int currTriggered { get; private set; }
    private LinkedList<Pickup> currItems;
    private LinkedListNode<Pickup> current = null;

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

    public void pickupActive()
    {
        // If items available pick it up
        if(currTriggered > 0)
        {
            current.Value.pickupItem();
            switchActive();
        }
    }

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
    }
}