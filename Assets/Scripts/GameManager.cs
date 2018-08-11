using UnityEngine;

public class GameManager : MonoBehaviour {

    private GameManager instance;

    GameManager getInstance()
    {
        return null;
    }


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
