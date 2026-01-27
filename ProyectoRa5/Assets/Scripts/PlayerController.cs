using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour moveBehaviour;
    public Animator animator;

    private Vector2 moveInput;
    private bool isSprinting = false;
    private bool isDancing = false;

    [Header("Camera Reference")]
    public CameraControllers cameraTransform;

    private void Awake()
    {
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
        // Opción A: NO cancelar baile al mover
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

            // Forzar que no haya movimiento
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

    private void LateUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 🔒 BLOQUEAR MOVIMIENTO MIENTRAS BAILA
        if (isDancing)
        {
            animator.SetFloat("speed", 0);

            // Detectar si la animación de baile terminó
            if (stateInfo.IsName("Dance") && stateInfo.normalizedTime >= 1f)
            {
                isDancing = false;
                animator.SetBool("dance", false);
            }

            return; // No permitir movimiento
        }

        // Movimiento normal
        moveBehaviour.MoveCharacter(moveInput, cameraTransform.CurrentCameraTransform, isSprinting);

        if (moveBehaviour.IsGrounded())
            animator.SetBool("jump", false);

        if (moveInput == Vector2.zero)
            animator.SetFloat("speed", 0);
        else if (!isSprinting)
            animator.SetFloat("speed", 0.5f);
        else
            animator.SetFloat("speed", 1f);
    }
}
