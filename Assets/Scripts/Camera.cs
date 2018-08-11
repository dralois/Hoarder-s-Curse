using UnityEngine;

public class Camera : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float lerpSpeed;

	void Update () {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, target.position, lerpSpeed), Quaternion.identity);
	}
}