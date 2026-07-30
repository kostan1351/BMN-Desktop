using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls free-flight movement and first-person camera rotation.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FlyingController : MonoBehaviour
{
    private const string MainMenuSceneName = "0-MainScence";

    public float flySpeed = 10f;
    public float speedMultiplier = 2f;
    public float mouseSensitivity = 2f;

    private Rigidbody playerRigidbody;
    private Transform cameraTransform;
    private Vector3 movementInput;
    private float currentSpeed;
    private float cameraPitch;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("FlyingController requires a child Camera.", this);
            enabled = false;
            return;
        }

        cameraTransform = playerCamera.transform;
        playerRigidbody.useGravity = false;
        playerRigidbody.isKinematic = false;
    }

    private void Start()
    {
        currentSpeed = flySpeed;
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(MainMenuSceneName);
            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }

        ReadMovementInput();

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            RotatePlayerAndCamera();
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.right * movementInput.x
            + transform.up * movementInput.y
            + transform.forward * movementInput.z;

        playerRigidbody.linearVelocity = movement.sqrMagnitude > 0f
            ? movement.normalized * currentSpeed
            : Vector3.zero;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isActiveAndEnabled)
        {
            LockCursor();
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ReadMovementInput()
    {
        float verticalMovement = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            verticalMovement += 1f;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            verticalMovement -= 1f;
        }

        movementInput = new Vector3(
            Input.GetAxis("Horizontal"),
            verticalMovement,
            Input.GetAxis("Vertical"));

        currentSpeed = Input.GetKey(KeyCode.LeftShift)
            ? flySpeed * speedMultiplier
            : flySpeed;
    }

    private void RotatePlayerAndCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch = Mathf.Clamp(cameraPitch - mouseY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
