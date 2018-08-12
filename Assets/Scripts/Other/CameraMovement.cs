using System.Collections;
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
        Vector3 newPos = new Vector3(Vector3.Lerp(transform.position, target.position, lerpSpeedX * Time.deltaTime).x,
                                    Vector3.Lerp(transform.position, target.position, lerpSpeedY * Time.deltaTime).y);
        // Only positive movement allowed
        newPos.x = newPos.x > transform.position.x ? newPos.x : transform.position.x;
        // Set position
        transform.SetPositionAndRotation(newPos, Quaternion.identity);        
	}

    /// <summary>
    /// Applies a smooth camera shake
    /// </summary>
    public void ApplyShake()
    {
        StartCoroutine(ShakePosition(transform, transform.position, .2f, 10f, .5f));
    }

    IEnumerator ShakePosition(Transform transform, Vector3 originalPosition, float duration, float speed, float magnitude)
    {
        float elapsedTime = 0f;
        // For the duration..
        while (elapsedTime < duration)
        {
            // Adjust time
            elapsedTime += Time.deltaTime;
            // Generate perlin noise offsets
            float x = (Mathf.PerlinNoise(Time.time * speed, 0f) * magnitude) - (magnitude / 2f);
            float y = (Mathf.PerlinNoise(0f, Time.time * speed) * magnitude) - (magnitude / 2f);
            // Apply
            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            // Yield
            yield return null;
        }
        // Revert to original position
        transform.localPosition = originalPosition;
    }
}