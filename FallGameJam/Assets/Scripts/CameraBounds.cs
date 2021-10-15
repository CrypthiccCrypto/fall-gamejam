using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothFactor;
    public Vector3 offset;

    void FixedUpdate() {
        moveCam();
    }

    void moveCam() {
        Vector3 targetPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
