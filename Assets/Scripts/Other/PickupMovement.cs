using UnityEngine;

public class PickupMovement : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;

    void Update()
    {
        // Spin it around the Y axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}