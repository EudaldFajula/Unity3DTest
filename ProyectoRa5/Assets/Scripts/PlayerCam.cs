using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllers : MonoBehaviour, InputSystem_Actions.ICameraControllersActions
{
    [Header("Cameras")]
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;
    public Camera mainCamera;
    public float FirstPersonYRotation => rotationY;

    [Header("Camera Layers")]
    public LayerMask firstPersonCullingMask;
    public LayerMask thirdPersonCullingMask;

    [Header("Sensitivity")]
    [SerializeField] private float _senseX;
    [SerializeField] private float _senseY;

    [Header("Player Reference")]
    public PlayerController playerController;

    private InputSystem_Actions inputActions;
    private float rotationX;
    private float rotationY;
    public Transform CurrentCameraTransform = null;
    public Animator cameraAnimator;
    public bool isFirstPerson = false;
    [SerializeField] private InputActionReference _lookAction;
    private Vector2 _lookInput;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.CameraControllers.SetCallbacks(this);
    }

    public void OnEnable()
    {
        inputActions.Enable();
        _lookAction.action.Enable();
        _lookAction.action.performed += OnLook;
        _lookAction.action.canceled += OnLook;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        _lookAction.action.performed -= OnLook;
        _lookAction.action.canceled -= OnLook;
        _lookAction.action.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Aplica el culling mask según el estado inicial
        mainCamera.cullingMask = isFirstPerson ? firstPersonCullingMask : thirdPersonCullingMask;
    }

    void Update()
    {
        if (isFirstPerson)
        {
            float mouseX = _lookInput.x * _senseX;
            float mouseY = _lookInput.y * _senseY;

            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -67.5f, 67.5f);
        }
    }

    private void LateUpdate()
    {
        if (isFirstPerson)
        {
            FirstPersonCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }

    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        if (playerController.isDancing) return; // No permite cambiar camara mientras baila

        isFirstPerson = !isFirstPerson;
        if (isFirstPerson)
        {
            cameraAnimator.Play("FirstPerson");
            CurrentCameraTransform = FirstPersonCam.transform;
            mainCamera.cullingMask = firstPersonCullingMask;
        }
        else
        {
            cameraAnimator.Play("ThirdPerson");
            CurrentCameraTransform = ThirdPersonCam.transform;
            mainCamera.cullingMask = thirdPersonCullingMask;
        }
    }
    public void SetDanceCamera(bool isDancing)
    {
        if (isDancing)
            mainCamera.cullingMask = thirdPersonCullingMask; // La camara de baile siempre muestra al player
        else
            mainCamera.cullingMask = isFirstPerson ? firstPersonCullingMask : thirdPersonCullingMask;
    }
    public void OnLook(InputAction.CallbackContext ctx)
    {
        _lookInput = ctx.ReadValue<Vector2>();
    }
}