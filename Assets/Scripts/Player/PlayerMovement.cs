using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpSpeed;
    
    private SpriteRenderer playerRenderer;
    private Collider2D playerCollider;
    private Rigidbody2D playerRB;
    private Animator playerAnim;    

    private void Start()
    {
        // Retrieve renderer
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Retrieve collider
        playerCollider = gameObject.GetComponent<Collider2D>();
        // Retrieve RB
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        // Retrieve animator
        playerAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerManager.Instance.isAlive)
        {
            // Get current input direction
            float moveDir = Input.GetAxis("Horizontal");
            // Adjust animation state
            playerAnim.SetBool("Walking", moveDir != 0);        
            // Adjust sprite facing direction
            playerRenderer.flipX = moveDir < 0;
            // Get contact points
            ContactPoint2D[] arr = new ContactPoint2D[10];
            int allContacts = playerCollider.GetContacts(arr);
            // Count only ground contacts
            int contactCount = arr.Take(allContacts).Count(curr => curr.collider.tag != "Wall" && curr.collider.tag != "MainCamera");
            // Move horizontally
            playerRB.velocity = new Vector2(moveDir * moveSpeed, playerRB.velocity.y);    
            // Handle jump
            if (Input.GetButtonDown("Jump"))
            {
                // If contacts or no vertical velocity then apply force
                if(playerRB.velocity.y == 0 || contactCount > 0)
                    playerRB.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse); 
            }
        }
    }
}