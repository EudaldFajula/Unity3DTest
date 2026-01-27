using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;


    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void MoveCharacter(Vector2 input, Transform cameraTransform, bool isSprinting)
    {
        // Movimiento relativo a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;

        // Elegir velocidad según sprint
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Aplicar velocidad
        Vector3 velocity = new Vector3(
            move.normalized.x * currentSpeed,
            _rb.linearVelocity.y,
            move.normalized.z * currentSpeed
        );

        _rb.linearVelocity = velocity;

        // Rotación del personaje
        Vector3 lookDirection = new Vector3(move.x, 0, move.z);

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
       

        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance);

        return Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
    }

}
