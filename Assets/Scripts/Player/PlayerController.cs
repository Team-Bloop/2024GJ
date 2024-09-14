using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    [Min(1)]
    private int stormDestroyExp;

    private PlayerInputActions input = null;
    private Vector2 moveVector = Vector2.zero;
    private float moveSpeed = 0f;

    private PlayerManager playerManager;
    private PlayerAbilityManager playerAbility;

    private void Awake()
    {
        input = new PlayerInputActions();
        playerManager = GetComponent<PlayerManager>();
        moveSpeed = playerManager.MovementSpeed;
        playerAbility = GetComponent<PlayerAbilityManager>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMovementPerformed;
        input.Player.Move.canceled += OnMovementCanceled;
        input.Player.Attack.performed += OnAttack;
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

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
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
        if (playerManager.Charges > 0 && playerAbility.DestroyStorm())
        {
            playerManager.IncreaseEXP(stormDestroyExp);
            playerManager.IncreaseCharges(-1);
        }
    }
}
