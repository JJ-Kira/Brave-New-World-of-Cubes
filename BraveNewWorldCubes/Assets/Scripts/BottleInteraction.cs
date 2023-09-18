using UnityEngine;

public class BottleInteraction : MonoBehaviour
{
    public Transform headTransform; // Reference to the main camera's transform
    public float detectionDistance = 0.2f; // Distance threshold for detecting proximity to head
    public float tiltAngleThreshold = 30f; // Tilt angle threshold for detecting the tilt gesture

    [SerializeField] private CubeManager cubeManager;

    private bool isHeld = false;

    void Start()
    {
        if (headTransform == null)
        {
            headTransform = Camera.main.transform; // Assuming the main camera is tagged as "MainCamera"
        }
    }

    void Update()
    {
        // Calculate the distance between the bottle and the head
        float distanceToHead = Vector3.Distance(transform.position, headTransform.position);

        // Check if the bottle is within the detection distance
        if (distanceToHead <= detectionDistance)
        {
            // Calculate the angle between the bottle's forward direction and the head's forward direction
            float angle = Vector3.Angle(transform.forward, headTransform.forward);

            // Check if the angle is within the tilt angle threshold
            if (angle <= tiltAngleThreshold)
            {
                if (!isHeld)
                {
                    // Bottle is held next to head and tilted
                    // You can place your desired actions or events here
                    Debug.Log("Player is drinking from the bottle!");
                    isHeld = true;
                    cubeManager.ShuffleVariants();
                }
            }
            else
            {
                isHeld = false;
            }
        }
        else
        {
            isHeld = false;
        }
    }
}

