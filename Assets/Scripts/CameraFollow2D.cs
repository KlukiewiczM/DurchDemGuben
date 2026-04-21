using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool useCurrentOffsetOnStart = true;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = false;

    private Vector3 velocity = Vector3.zero;
    private float lockedY;

    private void Start()
    {
        if (target == null) return;

        if (useCurrentOffsetOnStart)
        {
            offset = transform.position - target.position;
        }

        lockedY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;

        if (followX)
            desiredPosition.x = target.position.x + offset.x;

        if (followY)
            desiredPosition.y = target.position.y + offset.y;
        else
            desiredPosition.y = lockedY;

        desiredPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}