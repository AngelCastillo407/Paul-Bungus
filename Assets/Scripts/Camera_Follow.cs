using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Drag your character here
    public Vector3 offset;    // Set this in the Inspector

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }
}
