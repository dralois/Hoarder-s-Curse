using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    [Header("Specs")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float jumpHeight;

    // SpriteRenderer for adjusting animation
    private SpriteRenderer playerRenderer;
    private Animator playerAnim;
    // Jumping vars
    private float jumpTarget;
    private float gravityScale;

    [SerializeField]
    private int contacts;

    private void Start()
    {
        // Retrieve renderer
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Retrieve animator
        playerAnim = gameObject.GetComponent<Animator>();
    }

    void Update ()
    {
        // Get current input direction
        float moveDir = Input.GetAxis("Horizontal");
        // Adjust animation state
        playerAnim.SetBool("Walking", moveDir != 0);        
        // Adjust sprite facing direction
        playerRenderer.flipX = moveDir < 0;
        // Get contact points
        ContactPoint2D[] arr = new ContactPoint2D[10];
        contacts = gameObject.GetComponent<Collider2D>().GetContacts(arr);
        // If number of horizontal contact points greater zero no movement allowed
        if (arr.Take(contacts).LongCount(curr => Mathf.Sign(curr.normal.x) == -Mathf.Sign(moveDir) && Mathf.Abs(curr.normal.x) > .5f) == 0)
        {
            // Move along the horizontal axis
            transform.Translate(Vector3.right * moveDir * moveSpeed * Time.deltaTime);
        }
        // Smooth jump if remaining distance
        if(jumpTarget > 0)
        {
            // Calculate jump translation
            Vector3 translation = Vector3.up * jumpSpeed * Time.deltaTime;
            // If target reached
            if(jumpTarget - translation.magnitude < 0)
            {
                // Reapply gravity
                gameObject.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
                // Move upwards
                transform.Translate(Vector3.up * jumpTarget);
                jumpTarget = 0;
            }
            else
            {
                // Move up, adjust remaining distance
                jumpTarget -= translation.magnitude;
                transform.Translate(translation);
            }
        }
        // Otherwise check for jump
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Set target, save gravity scale, disable gravity
                jumpTarget = jumpHeight;
                gravityScale = gameObject.GetComponent<Rigidbody2D>().gravityScale;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

	}
}