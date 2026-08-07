using UnityEngine;

public class Camera2D : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform targetPlayer;
    public Vector3 offset = new Vector3(6f, 0f, -10f);

    [Header("Seguimiento")]
    public float smoothTime = 0.2f;
    public bool followVertical;

    [Header("Limites horizontales")]
    public bool useHorizontalLimits = true;
    public float minX = 0f;
    public float maxX = 7.5f;

    private Vector3 cameraVelocity;

    private void LateUpdate()
    {
        if (targetPlayer == null)
        {
            return;
        }

        float targetX = targetPlayer.position.x + offset.x;
        if (useHorizontalLimits)
        {
            targetX = Mathf.Clamp(targetX, minX, maxX);
        }

        float targetY = followVertical
            ? targetPlayer.position.y + offset.y
            : offset.y;

        Vector3 targetPosition = new Vector3(targetX, targetY, offset.z);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref cameraVelocity,
            Mathf.Max(0.01f, smoothTime)
        );
    }
}
