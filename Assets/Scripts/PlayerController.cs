using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves forward/backward, rotates with WASD/Arrow keys, and visually steers front wheels.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Налаштування руху (Movement Settings)")]
    public float speed = 5.0f;
    public float rotationSpeed = 120.0f;
    public float jumpForce = 4.0f;

    [Header("Налаштування коліс (Wheel Settings)")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public float maxSteerAngle = 30f;
    public float wheelTurnSpeed = 10f;

    private Rigidbody rb;
    private float currentSteerAngle = 0f;

    // Змінні для збереження початкового повороту коліс
    private Vector3 frontLeftWheelStartRot;
    private Vector3 frontRightWheelStartRot;
    private Vector3 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogWarning("PlayerController needs a Rigidbody.");

        // Запам'ятовуємо початковий поворот коліс, щоб вони не "падали" на бік
        if (frontLeftWheel != null)
            frontLeftWheelStartRot = frontLeftWheel.localEulerAngles;
        if (frontRightWheel != null)
            frontRightWheelStartRot = frontRightWheel.localEulerAngles;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        Vector2 moveInput = Vector2.zero;

        // Forward/backward
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveInput.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveInput.y = -1f;

        // Left/right (rotation)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1f;

        // --- ФІЗИКА РУХУ МАШИНИ ---
       if (Keyboard.current.wKey.isPressed && Keyboard.current.shiftKey.isPressed)
        {
            movement = transform.forward * moveInput.y * speed*1.5f * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
            movement = transform.forward * moveInput.y * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float turnDirection = moveInput.x;
        if (moveInput.y < 0)
            turnDirection = -turnDirection;

        float turn = turnDirection * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // --- ВІЗУАЛЬНИЙ ПОВОРОТ КОЛІС ---
        if (frontLeftWheel != null && frontRightWheel != null)
        {
            float targetAngle = moveInput.x * maxSteerAngle;
            currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetAngle, Time.fixedDeltaTime * wheelTurnSpeed);

            frontLeftWheel.localRotation = Quaternion.Euler(
                frontLeftWheelStartRot.x,
                frontLeftWheelStartRot.y + currentSteerAngle,
                frontLeftWheelStartRot.z
            );

            frontRightWheel.localRotation = Quaternion.Euler(
                frontRightWheelStartRot.x,
                frontRightWheelStartRot.y + currentSteerAngle,
                frontRightWheelStartRot.z
            );
        }
    }
}