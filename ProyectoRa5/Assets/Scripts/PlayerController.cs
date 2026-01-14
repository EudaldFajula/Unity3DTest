using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour moveBehaviour;

    private Vector2 moveInput;

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
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveBehaviour.JumpCharacter();
        }
    }

    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnDance(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }

    private void LateUpdate()
    {
        moveBehaviour.MoveCharacter(moveInput, cameraTransform.CurrentCameraTransform);
    }
}
