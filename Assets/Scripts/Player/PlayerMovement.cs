using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region MovementVariables
    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform playerCamera;
    #endregion

    #region JumpVariables
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    #endregion

    #region Components
    private PlayerInputActions playerControls;
    private Rigidbody rb;
    #endregion

    #region inputActions
    private InputAction move;
    private InputAction jump;
    #endregion

    #region States
    private bool isGrounded;
    #endregion

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetMoveInput();
        SetJumpInput();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
    }

    public void Move()
    {
        Vector2 input = move.ReadValue<Vector2>();
        if (input.magnitude > 0f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(rb.position + moveDir.normalized * speed * 2f * Time.deltaTime);
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }

    private void SetMoveInput()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void SetJumpInput()
    {
        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }
}
