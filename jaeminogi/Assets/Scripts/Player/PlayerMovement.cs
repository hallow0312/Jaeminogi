using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("BaseMovement")]
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpHeight = 3f;

    [Header("CheckGround")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundDistance;
    [SerializeField] float _gravity = -9.81f * 2f;

    [Header("PlayerCamera")]
    [SerializeField] Transform _cameraTransform;

    [Header("ModelTransform")]
    [SerializeField] Transform _modelTransform;







    bool _isGrounded;
     CharacterController _controller;
     Vector3 _velocity;
     Vector3 _moveInput = Vector3.zero;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;


        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();


        Vector3 move = camForward * _moveInput.z + camRight * _moveInput.x;
        _controller.Move(move * _speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        RotateModel();
        _controller.Move(_velocity * Time.deltaTime);
    }
    void RotateModel()
    {
        Vector3 move = _controller.velocity;
        move.y = 0;

        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            _modelTransform.rotation = Quaternion.Slerp(_modelTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck == null)
            return;

        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundDistance);
    }
    public void SetMoveInput(Vector3 moveInput)
    {
        _moveInput = moveInput;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}
