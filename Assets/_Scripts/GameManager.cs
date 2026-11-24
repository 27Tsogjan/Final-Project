/*  GameManager.cs
    Manages overall game state.
    Communicates with MenuManager.cs for scene transitions.

    @Janice: 
        Audio files:
        explorationMusic
        
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public MenuManager menuManager;

    // Background music
    public AudioClip explorationMusic;

    // Stops game when player loses or completes all levels
    private bool _gameIsOver = false;
    public bool GameIsOver => _gameIsOver;

    // Keep track of objectives
    public ObjectiveTracker objectiveTracker;

    public enum StoryProgress { FindMidBoss, FindFinalBossDoor, ReachFinalBoss, DefeatFinalBoss }
    public StoryProgress currentProgress = StoryProgress.FindMidBoss;

    private void Awake() {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start() {
        // Play exploration music
        if (SoundManager.Instance != null && explorationMusic != null) {
            SoundManager.Instance.PlayMusic(explorationMusic);
        }

        // Keep track of objectives
        if (objectiveTracker != null) {
            objectiveTracker.SetObjective("Find and defeat the midboss.");
        }

    }

    public void GameOver() {
        if (_gameIsOver) {
            return;
        }
        _gameIsOver = true;
        Time.timeScale = 1f;

        if (menuManager != null) {
            menuManager.OpenGameOverScreen();
        }

        else {
            Debug.LogError("GameManager: MenuManager is missing!");
        }
    }

    public void WinGame() {
        if (_gameIsOver) {
            return;
        }
        _gameIsOver = true;
        Time.timeScale = 1f;

        if (menuManager != null) {
            menuManager.OpenWinScreen();
        }

        else {
            Debug.LogError("GameManager: MenuManager is missing!");
        }
    }
}