using UnityEngine;

public class EndTile : MonoBehaviour
{
    [SerializeField]
    private int nextLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            LevelManager.Instance.LoadLevel(nextLevel);        
    }
}