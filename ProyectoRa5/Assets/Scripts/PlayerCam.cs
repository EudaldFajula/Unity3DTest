using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllers : MonoBehaviour, InputSystem_Actions.ICameraControllersActions
{
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;
    public bool ChangeCamera = true;
    private InputSystem_Actions inputActions;
    [SerializeField] private float _senseX;
    [SerializeField] private float _senseY;
    private float rotationX;
    private float rotationY;
    private void ChangeFirstPerson()
    {
        FirstPersonCam.SetActive(true);
        ThirdPersonCam.SetActive(false);
    }
    private void ChangeThirdPerson()
    {
        FirstPersonCam.SetActive(false);
        ThirdPersonCam.SetActive(true); 
    }
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.CameraControllers.SetCallbacks(this);
    }

    public void OnChangeCamera(InputAction.CallbackContext context)
    {
        ChangeCamera = !ChangeCamera;
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }
    void Update()
    {
        if (ChangeCamera)
        {
            ChangeThirdPerson();
        }
        else
        {
            ChangeFirstPerson();
            float mouseX = Input.GetAxisRaw("Mouse X") * _senseX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * _senseY;
            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -67.5f, 67.5f);
        }
    }
    private void LateUpdate()
    {
        if (!ChangeCamera)
        {
            FirstPersonCam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }


}
