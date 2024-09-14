using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class INTERACTIVEBUTTONS : MonoBehaviour 
{
    private PlayerInputActions input = null;
    [SerializeField]
    private GameObject settingsMenu;

    // Start is called before the first frame update
    void Awake() 
    {
        input = new PlayerInputActions();
    }

    private void OnEnable() {
        input.Enable();
        input.Player.Pause.performed += OnPause;
    }

    private void OnDisable() {
        input.Disable();
    }

    // Can move this functionality to a separate script when there is more time LOL
    private void OnPause(InputAction.CallbackContext context) {
        pauseToggle();
    }
    public void pauseToggle() {
        AudioManager.PlaySound(SoundType.UI_BUTTON_INTERACT);
        if (settingsMenu.activeSelf == false) {
            // Debug.Log("Pause");
            // pause
            settingsMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            // Debug.Log("Play");
            // play
            settingsMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void StartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Peanut_Scene");
    }
}
