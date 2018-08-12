using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour {

    private Transform _playerTarget;
    private SpriteRenderer _enemyRenderer;
    private Collider2D _enemyCollider;
    private Rigidbody2D _enemyRB;

    [SerializeField]
    private float _moveSpeedX;
    [SerializeField]
    private float _jumpSpeed;
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

    }
	
	// Update is called once per frame
	void Update () {

        if (_playerTarget != null)
        {
            Vector2 playerDirection = new Vector2(_playerTarget.position.x - gameObject.transform.position.x,
                                                  _playerTarget.position.y - gameObject.transform.position.y).normalized;
            
            _enemyRB.velocity = new Vector2(Math.Sign(playerDirection.x) * _moveSpeedX, _enemyRB.velocity.y);

            /*
            ContactPoint2D[] arr = new ContactPoint2D[10];
            int allContacts = _enemyCollider.GetContacts(arr);
            // Count only ground contacts
            int currContacts = arr.Take(allContacts).Where(curr => curr.collider.tag == "Ground" && (Mathf.Abs(Vector2.Angle(Vector2.up, curr.normal)) >= 1f)).Count();

            if (currContacts != 0)
                Jump();
            */
        }
	}

    private void Jump()
    {
        _enemyRB.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    }

    public void SetPlayerTarget(Transform transform)
    {
        _playerTarget = transform;
    }
}