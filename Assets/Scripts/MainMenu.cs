using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource musicManager;
    [SerializeField] AudioClip TitleTheme;

    PlayerControlsScript playerControlScript;

    public void EnableControls()
    {
        playerControlScript.MainMenu.Enable();

        playerControlScript.MainMenu.PlayGame.performed += PlayGame;

        playerControlScript.MainMenu.QuitGame.performed += QuitGame;
    }

    public void PlayThemeSong()
    {
        musicManager.Stop();
        musicManager.clip = TitleTheme;
        musicManager.loop = true;

        musicManager.Play();
    }

    private void Awake()
    {
        playerControlScript = new PlayerControlsScript();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PlayGame(InputAction.CallbackContext context)
    {
        playerControlScript.MainMenu.Disable();

        SceneManager.LoadSceneAsync(1);
    }

    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
