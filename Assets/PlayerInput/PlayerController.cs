using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private PlayerInputActions input = null;
    private Vector2 moveVector = Vector2.zero;
    private float moveSpeed = 0f;

    [SerializeField]
    private GameObject pauseMenu;

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
        if (playerManager.Charges > 0 && playerAbility.DestroyStorm())
        {
            playerManager.IncreaseCharges(-1);
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        pauseToggle();
    }

    public void pauseToggle() {
        if (pauseMenu.activeSelf == false) {
            // Debug.Log("Pause");
            // pause
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            // Debug.Log("Play");
            // play
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
