using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;   // Point Of Interest: what the camera follows

    [Header("Inscribed")]
    public float easing = 0.05f;          // How smoothly the camera catches up
    public Vector2 minXY = Vector2.zero;  // Don't let the camera go below/left of this

    [Header("Dynamic")]
    public float camZ;                    // The camera's starting Z position

    void Awake()
    {
        camZ = transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 destination;

        if (POI == null)
        {
            // Nothing to follow: head back toward the slingshot area
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position;

            // Once the projectile stops moving, stop following it
            if (POI.TryGetComponent(out Projectile proj) && !proj.awake)
            {
                POI = null;
                return;
            }
        }

        // Keep the camera from dipping below the ground or behind the slingshot
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        // Ease from the current position toward the destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;

        // Zoom out as the projectile goes higher, so the ground stays in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}