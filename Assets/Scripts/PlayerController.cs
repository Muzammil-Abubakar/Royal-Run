using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float xClamp = 3.7f;
    [SerializeField] private float zMinClamp = -1f;
    [SerializeField] private float zMaxClamp = 1.5f;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y)
            * moveSpeed * Time.fixedDeltaTime;

        Vector3 targetPosition = rb.position + movement;

        // X: -3.7 to 3.7
        targetPosition.x = Mathf.Clamp(targetPosition.x, -xClamp, xClamp);

        // Z: -1 to 1.5
        targetPosition.z = Mathf.Clamp(targetPosition.z, zMinClamp, zMaxClamp);

        rb.MovePosition(targetPosition);
    }
}