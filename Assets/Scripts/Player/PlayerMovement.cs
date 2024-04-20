using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    #region MovementVariables
    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform playerCamera;
    public Transform head;
    #endregion

    #region Components
    private PlayerInputActions playerControls;
    private Animator animator;
    private Rigidbody rb;
    #endregion

    #region inputActions
    private InputAction move;
    private InputAction jump;
    #endregion

    #region States
    private bool isGrounded;
    #endregion

    public Transform hand;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false; 
            return;
        }
    }
    private void Awake()
    {
        playerControls = new PlayerInputActions();

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        FindObjectOfType<CinemachineVirtualCamera>().Follow = head.transform;
    }

    private void OnEnable()
    {
        SetMoveInput();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector2 input = move.ReadValue<Vector2>();
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);
        if (input.magnitude > 0f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = transform.TransformDirection(moveDir); // Transform the direction relative to player's rotation
            rb.MovePosition(rb.position + moveDir.normalized * speed * 2f * Time.deltaTime);
        }
    }

    private void SetMoveInput()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }
}
