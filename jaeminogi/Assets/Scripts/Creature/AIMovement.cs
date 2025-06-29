using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AI_Movement : MonoBehaviour
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

        rb.useGravity = true;  
        rb.isKinematic = false;

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
        _prevBool = isRun;
    }

    void AIMove()
    {
        if (_isWalking)
        {
            _walkCounter -= Time.fixedDeltaTime;

            // ȸ�� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);

            // �̵� ���� ���� (�߷� ����)
            Vector3 velocity = transform.forward * _moveSpeed;
            velocity.y = rb.velocity.y; // �߷� ���� �״�� ����
            rb.velocity = velocity;

            UpdateRunningAnimation(true);

            if (_walkCounter <= 0f)
            {
                _isWalking = false;
                _waitCounter = Random.Range(2f, 4f);
                UpdateRunningAnimation(false);
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // �̵� ����, y�� �߷� ����
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
