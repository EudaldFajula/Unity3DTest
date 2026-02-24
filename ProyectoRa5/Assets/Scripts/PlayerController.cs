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
    //public para que CameraControllers pueda leerlo
    public bool isDancing = false;

    
    [SerializeField] private InteractBehavior interactBehavior;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private GameDataController gameDataController;
    private float velocity;

    [Header("Camera Reference")]
    //Script que controla las cámaras del juego
    public CameraControllers cameraControllers;
    public GameObject danceCam;
    private Transform previousCameraTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        moveBehaviour = GetComponent<MoveBehaviour>();
    }
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #region Metodos Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        // Si está bailando no permite moverse
        if (!isDancing)
            moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        // Solo salta si no está bailando
        if (context.started && !isDancing)
        {
            animator.SetBool("JumpRequest", true);
            moveBehaviour.JumpCharacter(transform);
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            interactBehavior.Interact();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            animator.SetTrigger("attack");
    }
    public void OnDance(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDancing = true;
            animator.SetBool("dance", true);
            // Para el movimiento mientras baila
            moveInput = Vector2.zero;

            previousCameraTransform = cameraControllers.CurrentCameraTransform;
            danceCam.SetActive(true);
            //Le dice a CameraControllers que use la cámara de baile
            cameraControllers.CurrentCameraTransform = danceCam.transform;
            cameraControllers.SetDanceCamera(true);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) isSprinting = true;
        if (context.canceled) isSprinting = false;
    }
    public void OnSaveGame(InputAction.CallbackContext context)
    {
        if (context.started)
            gameDataController.SaveData();
    }
    #endregion

    #region Metodos Update
    private void Update()
    {
        animator.SetFloat("speed", velocity);
    }
    private void FixedUpdate()
    {
        // Calcula la velocidad horizontal ignorando el eje Y (gravedad)
        velocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;

        // Mueve al personaje pasando:
        // - La dirección del input del jugador
        // - La cámara activa para orientar el movimiento
        // - Si está corriendo o no
        // - Si está en primera persona (cambia cómo se calcula el forward)
        // - La rotación horizontal de la cámara en primera persona
        moveBehaviour.MoveCharacter(
            moveInput,
            cameraControllers.CurrentCameraTransform,
            isSprinting,
            cameraControllers.isFirstPerson,
            cameraControllers.FirstPersonYRotation
        );
    }
    private void LateUpdate()
    {
        // Obtiene información del estado actual de la animación
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isDancing)
        {
            //Comprueba si la animación de baile ha terminado (normalizedTime >= 1 = 100%)
            if (stateInfo.IsName("Dance") && stateInfo.normalizedTime >= 1f)
            {
                isDancing = false;
                animator.SetBool("dance", false);

                danceCam.SetActive(false);
                cameraControllers.CurrentCameraTransform = previousCameraTransform;
                cameraControllers.SetDanceCamera(false);
            }
            return;
        }
        animator.SetBool("IsGrounded", moveBehaviour.IsGrounded(transform));
        animator.SetBool("JumpRequest", false);
    }
    #endregion
}