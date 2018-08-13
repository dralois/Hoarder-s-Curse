using System;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{

    private Transform _playerTarget;
    private SpriteRenderer _enemyRenderer;
    private Collider2D _enemyCollider;
    private Rigidbody2D _enemyRB;
    private float _lastHit;

    [Header("Movement")]
    [SerializeField]
    private float _moveSpeedX;
    [SerializeField]
    private float _moveSpeedY;
    [SerializeField]
    private float _minDistanceToPlayer;
    [Header("Damage Properties")]
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _damageInterval;
    [Header("Health")]
    [SerializeField]
    private int _maxHealth;
    private int _health;
    private void Start()
    {
        // Retrieve the renderer
        _enemyRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Retrieve collider
        _enemyCollider = gameObject.GetComponent<Collider2D>();
        // Retrieve RB
        _enemyRB = gameObject.GetComponent<Rigidbody2D>();
        // Set PlayerTarget to null
        _playerTarget = null;
        // Init the lastHit time to the damageInterval
        _lastHit = 0f;
        // Set the current health to maxHealth
        _health = _maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        if (_playerTarget != null)
        {
            _enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;

            Vector2 playerDirection = new Vector2(_playerTarget.position.x - gameObject.transform.position.x,
                                                  _playerTarget.position.y - gameObject.transform.position.y);

            RaycastHit2D raycastHit = Physics2D.Raycast(gameObject.transform.position, playerDirection, Mathf.Infinity, LayerMask.GetMask(new[] { "Wall", "Ground", "Default" }));

            if (raycastHit.collider.tag != "Player")
            {
                Vector2 moveDirection = UnityEngine.Random.insideUnitCircle;
                Move(moveDirection);
            }
            else
            {
                if (playerDirection.magnitude <= _minDistanceToPlayer)
                    Move(-playerDirection);
                else
                    Move(playerDirection);
                //Attack();
            }

            _enemyRenderer.flipX = (playerDirection.x < 0);

        }
        else
        {
            _enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void Move(Vector2 direction)
    {
        _enemyRB.velocity = new Vector2(Math.Sign(direction.x) * _moveSpeedX, Math.Sign(direction.y) * _moveSpeedY);
    }

    public void Attack()
    {
        _lastHit -= Time.fixedDeltaTime;
        if (_lastHit <= 0f)
        {
            PlayerManager.Instance.ApplyDamage(_damage);
            _lastHit = _damageInterval;
        }
    }

    public void ResetHitTime()
    {
        _lastHit = 0f;
    }

    public void ApplyDamage(int amount)
    {
        // Reduce or set to zero
        if (_health - amount > 0)
        {
            _health -= amount;
        }
        else
        {
            _health = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void SetPlayerTarget(Transform transform)
    {
        _playerTarget = transform;
    }
}
