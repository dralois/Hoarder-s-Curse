using UnityEngine;

public class EnemyGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<EnemyGround>();

        if(collision.tag == "Player")
        {

        }

    }
}