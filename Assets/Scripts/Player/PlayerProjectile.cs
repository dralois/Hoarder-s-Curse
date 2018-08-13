using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;

    public int damage;
    public float range;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Remove after range reached
        if(Mathf.Abs((transform.position - startPos).x) > range)
        {
            Destroy(gameObject);
        }
    }

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