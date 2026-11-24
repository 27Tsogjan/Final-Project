/*  PauseMenu.cs
    Creates a pause menu.

    @Janice: 
        Title Screen: Scene 0
        Character Select Screen: Scene 1
        Main Game: Scene 2
        Game Over: Scene 3
        Win Screen: Scene 4


        Audio files:
        buttonClickSFX
*/

using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
    public GameObject pauseMenu;
    public AudioClip buttonClickSFX;

    private void PlayButtonSound() {
        if (SoundManager.Instance != null && buttonClickSFX != null) {
            SoundManager.Instance.Play(buttonClickSFX);
        }
    }

    public void Pause() {
        PlayButtonSound();

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue() {
        PlayButtonSound();

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadRestart() {
        SceneManager.LoadScene(2);
    }

    public void Restart() {
        PlayButtonSound();

        Time.timeScale = 1f;
        Invoke("LoadRestart", 0.5f);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void MainMenu() {
        PlayButtonSound();

        Time.timeScale = 1f;
        Invoke("LoadMainMenu", 0.5f);
    }
}