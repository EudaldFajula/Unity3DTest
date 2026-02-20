using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllers : MonoBehaviour, InputSystem_Actions.ICameraControllersActions
{
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;
    private InputSystem_Actions inputActions;
    [SerializeField] private float _senseX;
    [SerializeField] private float _senseY;
    private float rotationX;
    private float rotationY;
    public Transform CurrentCameraTransform = null;
    public Animator cameraAnimator; 
    private bool isFirstPerson = false;
    [SerializeField] private InputActionReference _lookAction;
    private Vector2 _lookInput;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.CameraControllers.SetCallbacks(this);
    }

    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        isFirstPerson = !isFirstPerson;
        if (isFirstPerson)
        {
            cameraAnimator.Play("FirstPerson");
            CurrentCameraTransform = FirstPersonCam.transform;
        }
        else
        {
            cameraAnimator.Play("ThirdPerson");
            CurrentCameraTransform = ThirdPersonCam.transform;
        }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
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

    public void OnLook(InputAction.CallbackContext ctx)
    {
        _lookInput = ctx.ReadValue<Vector2>();
    }
}
