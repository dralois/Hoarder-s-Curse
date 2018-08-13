using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            gameObject.GetComponentInParent<EnemyFlying>().TargetHit();

        if (collision.tag == "Player" || collision.tag == "Ground" || collision.tag == "Wall")
            Destroy(gameObject);
    }
}