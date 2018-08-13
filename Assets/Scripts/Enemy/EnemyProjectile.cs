using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [Header("Movement")]
    [SerializeField]
    public float moveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            gameObject.GetComponentInParent<EnemyFlying>().TargetHit();

        if (collision.tag == "Player" || collision.tag == "Ground" || collision.tag == "Wall")
            Destroy(gameObject);
    }
}
