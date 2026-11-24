/*  GameOverScreen.cs
    Player was unable to defeat the final boss.
    
    @Janice: 
        Title Screen: Scene 0
        Character Select Screen: Scene 1
        Main Game: Scene 2
        Game Over: Scene 3
        Win Screen: Scene 4

        Audio files:
        gameOverMusic
        buttonClickSFX
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour 
{
    public AudioClip buttonClickSFX;
    public AudioClip gameOverMusic;
    
    public void Start() {
        if (SceneManager.GetActiveScene().buildIndex == 3 && SoundManager.Instance != null && gameOverMusic != null) {
            SoundManager.Instance.PlayMusic(gameOverMusic);
        }
    }

    public void LoadRestart() {
        SceneManager.LoadScene(2);
    }

    public void Restart() {
        if (SoundManager.Instance != null && buttonClickSFX != null) {
            SoundManager.Instance.Play(buttonClickSFX);
        }

        Time.timeScale = 1f;
        Invoke("LoadRestart", 0.5f);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void MainMenu() {
        if (SoundManager.Instance != null && buttonClickSFX != null) {
            SoundManager.Instance.Play(buttonClickSFX);
        }

        Time.timeScale = 1f;
        Invoke("LoadMainMenu", 0.5f);
    }
}
