using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyFlying : MonoBehaviour {

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

            Physics2D.Raycast((Vector2)gameObject.transform.position, playerDirection);

            _enemyRB.velocity = new Vector2(Math.Sign(playerDirection.x) * _moveSpeedX, _enemyRB.velocity.y);

            _enemyRenderer.flipX = (playerDirection.x < 0);

            /*
            ContactPoint2D[] arr = new ContactPoint2D[10];
            int allContacts = _enemyCollider.GetContacts(arr);
            // Count only ground contacts
            int currContacts = arr.Take(allContacts).Where(curr => curr.collider.tag == "Ground" && (Mathf.Abs(Vector2.Angle(Vector2.up, curr.normal)) >= 1f)).Count();

            if (currContacts != 0)
                Jump();
            */
        }
        else
        {
            _enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
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
