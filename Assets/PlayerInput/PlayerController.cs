using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private PlayerInputActions input = null;
    private Vector2 moveVector = Vector2.zero;
    private float moveSpeed = 0f;

    private void Awake()
    {
        input = new PlayerInputActions();
        moveSpeed = GetComponent<PlayerManager>().MovementSpeed;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCanceled;
        input.Player.Attack.performed += OnAttack;
        input.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Called at least zero times per frame depending on physics engine. 
    // This should only be used for physics based actions such as applying forces, torques, etc.
    // Otherwise use Update() instead.
    private void FixedUpdate()
    {
        rb.velocity = moveVector * moveSpeed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack");
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("Pause");
    }
}
