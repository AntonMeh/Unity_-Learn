using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("How many real-world seconds it takes for a full in-game day (360-degree rotation) to pass.")]
    [SerializeField] private float dayDurationInSeconds = 120f;

    void Update()
    {
        // Prevent errors if someone sets the duration to 0 in the Inspector
        if (dayDurationInSeconds <= 0f)
        {
            return;
        }

        // Calculate how many degrees to rotate per second (A full circle is 360 degrees)
        float degreesPerSecond = 360f / dayDurationInSeconds;

        // Calculate rotation for this specific frame
        float rotationThisFrame = degreesPerSecond * Time.deltaTime;

        // Rotate around the X-axis (Vector3.right) to simulate the sun rising and setting
        transform.Rotate(Vector3.right, rotationThisFrame);
    }
}