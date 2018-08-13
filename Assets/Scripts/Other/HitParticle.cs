using UnityEngine;

public class HitParticle : MonoBehaviour {

    private float duration = .2f;

	void Update () {
        if (duration > 0)
            duration -= Time.deltaTime;
        else
            Destroy(gameObject);		
	}
}