/*  WinScreen.cs
    Boss has been defeated, player has won the game.

    @Janice: 
        Title Screen: Scene 0
        Character Select Screen: Scene 1
        Main Game: Scene 2
        Game Over: Scene 3
        Win Screen: Scene 4

        Audio files:
        winScreenMusic
        buttonClickSFX

        Invokes may need to be changed, depending on audio clip length
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour 
{
    public AudioClip buttonClickSFX;
    public AudioClip winScreenMusic;

    public void Start() {
        if (SceneManager.GetActiveScene().buildIndex == 4 && SoundManager.Instance != null && winScreenMusic != null) {
            SoundManager.Instance.PlayMusic(winScreenMusic);
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
        Invoke(nameof(LoadMainMenu), 0.5f);
    }
}