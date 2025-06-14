using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float _speed = 5f;
    [SerializeField]float _jumpHeight = 3f;
    [SerializeField]float _gravity = -9.81f * 2f;
    [SerializeField]LayerMask groundMask;
    [SerializeField]Transform _groundCheck;
    [SerializeField]float _groundDistance = 1.38f;

     CharacterController _controller;
     Vector3 _moveInput = Vector3.zero;
     Vector3 _velocity;
     [SerializeField] bool _isGrounded;

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
        {
            _velocity.y = -2f;
        }

        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.z;
        _controller.Move(move * _speed * Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
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
