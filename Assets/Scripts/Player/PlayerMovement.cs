using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private Transform flipPoint;

    // Time off ground while still allowed to jump
    private float maxContactlessJump = .07f;
    private float currContactless;
    
    // Other stuff
    private SpriteRenderer playerRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D playerRB;
    private Animator playerAnim;    

    private void Start()
    {
        // Retrieve renderer
        playerRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        // Retrieve collider
        playerCollider = gameObject.GetComponent<Collider2D>();
        // Retrieve RB
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        // Retrieve animator
        playerAnim = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (PlayerManager.Instance.isAlive)
        {
            // Get current input direction
            float moveDir = Input.GetAxis("Horizontal");
            // Adjust animation state
            playerAnim.SetBool("Walking", moveDir != 0);
            // Adjust sprite facing direction and animation offset
            bool didFlip = moveDir < 0 ? true : moveDir > 0 ? false : playerRenderer.flipX;
            flipPoint.localPosition = didFlip == playerRenderer.flipX ? flipPoint.localPosition : -flipPoint.localPosition;
            playerRenderer.flipX = didFlip;
            // Get contact points
            Collider2D[] arr = new Collider2D[10];
            int allContacts = playerCollider.GetContacts(arr);
            // Count only ground contacts
            int contactCount = arr.Take(allContacts).Count(curr => curr.tag == "Ground");
            // If no contact at all
            if(contactCount == 0)
            {
                // Adjust remaining time to jump
                currContactless -= Time.deltaTime;
            }
            else
            {
                // Reset on ground touch
                if (currContactless <= 0)
                    currContactless = maxContactlessJump;
            }
            // Move horizontally
            playerRB.velocity = new Vector2(moveDir * moveSpeed, playerRB.velocity.y);    
            // Handle jump
            if (Input.GetButtonDown("Jump"))
            {
                // If jump still allowed or no vertical velocity then apply force
                if(playerRB.velocity.y == 0 || currContactless > 0)
                    playerRB.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse); 
            }
        }
    }
}