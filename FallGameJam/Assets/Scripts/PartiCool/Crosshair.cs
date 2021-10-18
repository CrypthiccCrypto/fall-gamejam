using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void Update() {
        followMouse();
    }

    void followMouse() {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPosition;
    }
}
