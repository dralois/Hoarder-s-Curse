using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickupManager : MonoBehaviour {

    // Singleton instance
    private static PickupManager _instance = null;

    // Current number of triggered items
    public int currTriggered { get; private set; }
    private LinkedList<Pickup> currItems;    

    public void enterItem(Pickup item)
    {
        currItems.AddLast(item);
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
    }
}