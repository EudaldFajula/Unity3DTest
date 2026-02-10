using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour moveBehaviour;
    public Animator animator;
    private Rigidbody _rb;
    private Vector2 moveInput;
    private bool isSprinting = false;
    private bool isDancing = false;

    // Miquel varbiable
    private float velocity;

    [Header("Camera Reference")]
    public CameraControllers cameraTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        moveBehaviour = GetComponent<MoveBehaviour>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !isDancing)
        {
            animator.SetBool("jump", true);
            moveBehaviour.JumpCharacter();
        }
    }

    public void OnAttack(InputAction.CallbackContext context) { }

    public void OnDance(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDancing = true;
            animator.SetBool("dance", true);

            moveInput = Vector2.zero;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            isSprinting = true;

        if (context.canceled)
            isSprinting = false;
    }
    private void Update()
    {
        animator.SetFloat("speed", velocity);
    }
    private void FixedUpdate()
    {
        velocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;
        moveBehaviour.MoveCharacter(moveInput, cameraTransform.CurrentCameraTransform, isSprinting);
    }
    private void LateUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isDancing)
        {
            //animator.SetFloat("speed", 0);

            if (stateInfo.IsName("Dance") && stateInfo.normalizedTime >= 1f)
            {
                isDancing = false;
                animator.SetBool("dance", false);
            }

            return;
        }

        

        if (moveBehaviour.IsGrounded())
            animator.SetBool("jump", false);
        /*
        if (moveInput == Vector2.zero)
            animator.SetFloat("speed", 0);
        else if (!isSprinting)
            animator.SetFloat("speed", 0.5f);
        else
            animator.SetFloat("speed", 1f);
        */
    }
}
