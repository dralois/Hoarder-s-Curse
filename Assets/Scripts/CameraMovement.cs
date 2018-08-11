using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [Header("Settings")]
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float lerpSpeedX;
    [SerializeField]
    private float lerpSpeedY;

    void Update () {
        // Lerp to position
        Vector3 newPos =    new Vector3(Vector3.Lerp(transform.position, target.position, lerpSpeedX * Time.deltaTime).x,
                                        Vector3.Lerp(transform.position, target.position, lerpSpeedY * Time.deltaTime).y);
        // Only positive movement allowed
        newPos.x = newPos.x > transform.position.x ? newPos.x : transform.position.x;
        // Set position
        transform.SetPositionAndRotation(newPos, Quaternion.identity);        
	}
}