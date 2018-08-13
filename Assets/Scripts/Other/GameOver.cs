using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private int toLoad;

    // Load first scene on button press
	void Update () {
        if (Input.anyKeyDown)
        {
            if(PlayerManager.Instance != null)
                PlayerManager.Instance.Revive();
            LevelManager.Instance.LoadLevel(toLoad);
        }
	}
}