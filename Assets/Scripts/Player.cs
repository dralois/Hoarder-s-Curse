using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    // SpriteRenderer for adjusting animation
    private SpriteRenderer playerRenderer;

    private void Start()
    {
        // Retrieve renderer
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update () {

        // Get current input direction
        float moveDir = Input.GetAxis("Horizontal");
        // Adjust axis
        playerRenderer.flipX = moveDir < 0;
        // Move player
        transform.Translate(Vector3.right * moveDir * moveSpeed * Time.deltaTime);
        // Check for jump
        if (Input.GetButtonDown("Jump"))
        {
            transform.Translate(Vector3.up);
        }
	}
}