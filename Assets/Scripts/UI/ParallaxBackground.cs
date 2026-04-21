using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxEffectX = 0.2f;
    [SerializeField] private float parallaxEffectY = 0f;
    [SerializeField] private bool snapToCameraOnStart = true;
    [SerializeField] private bool pixelSnap = false;
    [SerializeField] private float pixelsPerUnit = 16f;

    private Vector3 startBackgroundPosition;
    private Vector3 startCameraPosition;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogWarning("ParallaxBackground: brak Camera Transform.");
            enabled = false;
            return;
        }

        if (snapToCameraOnStart)
        {
            transform.position = new Vector3(
                cameraTransform.position.x,
                cameraTransform.position.y,
                transform.position.z
            );
        }

        startBackgroundPosition = transform.position;
        startCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        float targetX = startBackgroundPosition.x + (cameraTransform.position.x - startCameraPosition.x) * parallaxEffectX;
        float targetY = startBackgroundPosition.y + (cameraTransform.position.y - startCameraPosition.y) * parallaxEffectY;

        Vector3 newPosition = new Vector3(targetX, targetY, transform.position.z);

        if (pixelSnap && pixelsPerUnit > 0)
        {
            float unit = 1f / pixelsPerUnit;
            newPosition.x = Mathf.Round(newPosition.x / unit) * unit;
            newPosition.y = Mathf.Round(newPosition.y / unit) * unit;
        }

        transform.position = newPosition;
    }
}