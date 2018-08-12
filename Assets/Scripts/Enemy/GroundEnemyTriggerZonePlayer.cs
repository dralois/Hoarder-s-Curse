using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyTriggerZonePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponentInParent<EnemyGround>().SetPlayerTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponentInParent<EnemyGround>().SetPlayerTarget(null);
        }
    }
}
