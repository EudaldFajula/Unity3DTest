using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;
    private bool _isGrounded;
    [Header("Ground Check")]
    public LayerMask groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius = 0.3f;

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
            move.x * currentSpeed,
            _rb.linearVelocity.y,
            move.z * currentSpeed
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



    public void JumpCharacter(Transform transformPlayer)
    {
        if (IsGrounded(transformPlayer))
        {
            Vector3 v = _rb.linearVelocity;
            v.y = 0f;
            _rb.linearVelocity = v;

            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
        }
    }

    public bool IsGrounded(Transform transformPlayer)
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, groundLayer);
        return _isGrounded;
    }
    
}
