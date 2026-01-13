using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    public float speed = 6f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void MoveCharacter(Vector2 input, Transform cameraTransform)
    {
        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;

        // Aplicar velocidad
        Vector3 velocity = new Vector3(move.normalized.x * speed, _rb.linearVelocity.y, move.normalized.z * speed);
        _rb.linearVelocity = velocity;

        Vector3 lookDirection = new Vector3(move.x, 0, move.z);

        // evita rotación cuando no te mueves
        if (lookDirection.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }


    public void JumpCharacter()
    {
        if (IsGrounded())
        {
            Vector3 v = _rb.linearVelocity;
            v.y = 0f;
            _rb.linearVelocity = v;

            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
    }
}
