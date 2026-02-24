using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllers : MonoBehaviour, InputSystem_Actions.ICameraControllersActions
{
    [Header("Cameras")]
    public GameObject FirstPersonCam;
    public GameObject ThirdPersonCam;
    public Camera mainCamera;
    /*Propiedad que expone la rotación horizontal de la primera persona
    /para que MoveBehaviour pueda orientar el movimiento correctamente*/
    public float FirstPersonYRotation => rotationY;

    [Header("Camera Layers")]
    public LayerMask firstPersonCullingMask;
    public LayerMask thirdPersonCullingMask;

    [Header("Sensitivity")]
    //horizontal
    [SerializeField] private float _senseX;
    //vertical
    [SerializeField] private float _senseY;

    [Header("Player Reference")]
    //Referencia al PlayerController para saber si está bailando
    public PlayerController playerController;

    private InputSystem_Actions inputActions;
    private float rotationX;
    private float rotationY;
    //Transform de la cámara activa en cada momento, usado por MoveBehaviour para orientar el movimiento
    public Transform CurrentCameraTransform = null;
    public Animator cameraAnimator;
    //Esta en public para que PlayerController pueda leerlo
    public bool isFirstPerson = false;
    private Vector2 _lookInput;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.CameraControllers.SetCallbacks(this);
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        //Bloquea y oculta el cursor al iniciar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera.cullingMask = isFirstPerson ? firstPersonCullingMask : thirdPersonCullingMask;
    }

    void Update()
    {
        //Solo calcula la rotación en primera persona
        if (isFirstPerson)
        {
            //Multiplica el input del ratón por la sensibilidad
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
        //No permite cambiar de cámara mientras el jugador baila
        if (playerController.isDancing) return;

        //Alterna entre primera y tercera persona
        isFirstPerson = !isFirstPerson;

        if (isFirstPerson)
        {
            cameraAnimator.Play("FirstPerson");
            //Actualiza la cámara de referencia para el movimiento
            CurrentCameraTransform = FirstPersonCam.transform;
            //Oculta la layer del jugador en primera persona
            mainCamera.cullingMask = firstPersonCullingMask;
        }
        else
        {
            cameraAnimator.Play("ThirdPerson");
            //Actualiza la cámara de referencia para el movimiento
            CurrentCameraTransform = ThirdPersonCam.transform;
            //Muestra la layer del jugador en tercera persona
            mainCamera.cullingMask = thirdPersonCullingMask;
        }
    }
    public void SetDanceCamera(bool isDancing)
    {
        if (isDancing)
            //Durante el baile siempre se muestra al jugador (como en tercera persona)
            mainCamera.cullingMask = thirdPersonCullingMask;
        else
            //Al terminar el baile restaura el culling mask según la cámara activa
            mainCamera.cullingMask = isFirstPerson ? firstPersonCullingMask : thirdPersonCullingMask;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        _lookInput = ctx.ReadValue<Vector2>();
    }
}