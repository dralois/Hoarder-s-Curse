using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Load first scene on button press
	void Update () {
        if (Input.anyKeyDown)
        {
            PlayerManager.Instance.Revive();
            LevelManager.Instance.LoadLevel(1);
        }
	}
}