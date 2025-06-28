using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AI_Movement : MonoBehaviour  // SIMPLE AI MOVING
{
    private Animator animator;
    private Rigidbody rb;

    [SerializeField] float _moveSpeed = 5.0f;
    [SerializeField] float _rotationSpeed = 5f;

    private float _walkCounter;
    private float _waitCounter;

    private int _walkDirection;
    private bool _isWalking;

    private Quaternion targetRotation;

    private bool _prevBool;
    void Start()
    {
        _prevBool = false;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true; // 직접 힘을 주진 않지만, MovePosition 사용할 수 있도록 설정

        _waitCounter = Random.Range(2f, 4f);
        _walkCounter = 0;
    }

    void FixedUpdate()
    {
        AIMove();
    }
    void UpdateRunningAnimation(bool isRun)
    {
        if (_prevBool == isRun) return;
        animator.SetBool("isRunning", isRun);
    }
    void AIMove()
    {
        if (_isWalking)
        {
            _walkCounter -= Time.fixedDeltaTime;

            // 회전 보간
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);

            // 이동
            Vector3 moveDir = transform.forward * _moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDir);

            UpdateRunningAnimation(true);
            if (_walkCounter <= 0f)
            {
                _isWalking = false;
                _waitCounter = Random.Range(2f, 4f);
                UpdateRunningAnimation(false);
            }
        }
        else
        {
            _waitCounter -= Time.fixedDeltaTime;

            if (_waitCounter <= 0f)
            {
                ChooseDirection();
            }
        }
    }

    void ChooseDirection()
    {
        _walkDirection = Random.Range(0, 4);
        _walkCounter = Random.Range(2f, 4f);
        _isWalking = true;

        switch (_walkDirection)
        {
            case 0: targetRotation = Quaternion.Euler(0f, 0f, 0f); break;
            case 1: targetRotation = Quaternion.Euler(0f, 90f, 0f); break;
            case 2: targetRotation = Quaternion.Euler(0f, -90f, 0f); break;
            case 3: targetRotation = Quaternion.Euler(0f, 180f, 0f); break;
        }
    }
}
