/*  MenuManager.cs
    Manage screens: pause, win, and game over
    
    @Janice: 
        Title Screen: Scene 0
        Character Select Screen: Scene 1
        Main Game: Scene 2
        Game Over: Scene 3
        Win Screen: Scene 4

        Audio files:
        mainMenuMusic
        buttonClickSFX
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    [Header("Menu Scripts")]
    public PauseMenu pauseMenu;
    public WinScreen winScreen;
    public GameOverScreen gameOverScreen;

    public AudioClip mainMenuMusic;
    public AudioClip buttonClickSFX;

    private void Start() {
        if (SceneManager.GetActiveScene().buildIndex == 0 && SoundManager.Instance != null && mainMenuMusic != null) {
            SoundManager.Instance.PlayMusic(mainMenuMusic);
        }
    }

    // Pause Screen
    public void Pause() {
        pauseMenu.Pause();
    }

    public void Continue() {
        pauseMenu.Continue();
    }

    public void Restart() {
        pauseMenu.Restart();
    }

    public void MainMenu() {
        pauseMenu.MainMenu();
    }

    // Win Screen
    public void WinMainMenu() {
        winScreen.MainMenu();
    }

    public void WinRestart() {
        winScreen.Restart();
    }

    public void OpenWinScreen() {
        SceneManager.LoadScene(4);
    }

    // Game Over Screen
    public void GameOverRestart() {
        gameOverScreen.Restart();
    }

    public void GameOverMainMenu() {
        gameOverScreen.MainMenu();
    }

    public void OpenGameOverScreen() {
    SceneManager.LoadScene(3);
    }

    public void OpenCharacterSelect() {
        if (SoundManager.Instance != null && buttonClickSFX != null) {
            SoundManager.Instance.Play(buttonClickSFX);
        }
        
        Invoke("LoadCharacterSelect", 0.5f);
    }

    public void LoadCharacterSelect() {
        SceneManager.LoadScene(1);
    }
}