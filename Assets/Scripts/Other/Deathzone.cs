using UnityEngine;

public class Deathzone : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kill player on hit
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.Kill();
        }
    }
}