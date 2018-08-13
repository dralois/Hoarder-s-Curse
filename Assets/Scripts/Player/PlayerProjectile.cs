using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;

    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyFlying fly = collision.GetComponent<EnemyFlying>();
            EnemyGround ground = collision.GetComponent<EnemyGround>();
            if (fly == null)
                ground.ApplyDamage(damage);
            else
                fly.ApplyDamage(damage);
        }

        if (collision.tag == "Enemy" || collision.tag == "Ground" || collision.tag == "Wall")
            Destroy(gameObject);
    }
}