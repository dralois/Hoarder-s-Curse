using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour {

    private Transform _playerTarget;
    private SpriteRenderer _enemyRenderer;
    private Collider2D _enemyCollider;
    private Rigidbody2D _enemyRB;
    private float _lastHit;
    private float _knockback;
    private int _health;

    [Header("Movement")]
    [SerializeField]
    private float _moveSpeedX;
    [SerializeField]
    private float _jumpSpeed;
    [Header("Damage Properties")]
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _damageInterval;
    [Header("Health")]
    [SerializeField]
    private int _maxHealth;
    [Header("Drops")]
    [SerializeField]
    private List<InventoryItem> _dropList;
    [Header("Objects")]
    [SerializeField]
    private RectTransform healthSize;

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
	void Update ()
    {
        if (_knockback > 0)
        {
            _knockback -= Time.deltaTime;
            return;
        }

        if (_playerTarget != null)
        {
            _enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            Vector2 playerDirectionNormalized = new Vector2(_playerTarget.position.x - gameObject.transform.position.x,
                                                  _playerTarget.position.y - gameObject.transform.position.y).normalized;
            
            Move(playerDirectionNormalized);

            _enemyRenderer.flipX = (playerDirectionNormalized.x < 0);
        }
        else
        {
            _enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
	}

    private void Move(Vector2 direction)
    {
        _enemyRB.velocity = new Vector2(Math.Sign(direction.x) * _moveSpeedX, _enemyRB.velocity.y);
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
        _knockback = .2f;
        // Reduce or set to zero
        if (_health - amount > 0)
        {
            _health -= amount;
            SetHealthBar();
        }
        else
        {
            _health = 0;
            SetHealthBar();
            Die();
        }
    }

    private void Die()
    {
        PickupManager.Instance.SpawnItem(transform.position, _dropList);

        Destroy(gameObject);
    }

    private void SetHealthBar()
    {
        healthSize.localScale = new Vector3(_health/_maxHealth, 1, 1);
    }

    public void SetPlayerTarget(Transform transform)
    {
        _playerTarget = transform;
    }
}