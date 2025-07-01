using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("CameraSetting")]
    [SerializeField] float _mouseSensitivity = 0.1f;
    [SerializeField] float _maxVerticalAngle = 60.0f;
    [SerializeField] float _minVerticalAngle = -30.0f;
    LayerMask cameraCollision;
    Transform _gameObject;

    Camera _camera;
    Vector2 _lookInput;

    float _yaw;
    float _pitch;



    [SerializeField] Vector3 _offset = new Vector3(0, 2, -3.5f);

    PlayerInput _cameraInput;

    public Vector3 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set => _mouseSensitivity = value;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _gameObject = transform.parent;
        _cameraInput = new PlayerInput();
        cameraCollision = LayerMask.GetMask("CameraCollision");
    }

    private void OnEnable()
    {
        _cameraInput.Player.Look.performed += OnLook;
        _cameraInput.Player.Look.canceled += OnLookCanceled;
        _cameraInput.Player.Enable();
    }

    private void OnDisable()
    {
        _cameraInput.Player.Look.performed -= OnLook;
        _cameraInput.Player.Look.canceled -= OnLookCanceled;
        _cameraInput.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _lookInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        HandleCamera();
    }

    void HandleCamera()
    {
        float mouseX = _lookInput.x * _mouseSensitivity;
        float mouseY = _lookInput.y * _mouseSensitivity;

        _yaw += mouseX;

        _pitch = Mathf.Clamp(_pitch - mouseY, _minVerticalAngle, _maxVerticalAngle);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);


        Vector3 desiredPosition = _gameObject.position + rotation * _offset;
        float desiredDistance = _offset.magnitude;


        Vector3 rayDir = (desiredPosition - _gameObject.position).normalized;
        if (Physics.Raycast(_gameObject.position, rayDir, out RaycastHit hit, desiredDistance, cameraCollision))
        {
            float buffer = 0.1f;
            desiredPosition = hit.point - rayDir * buffer;
        }

        transform.position = desiredPosition;

        transform.LookAt(_gameObject.position + Vector3.up * 1.2f);
    }
}
