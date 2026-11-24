/*  CharacterSelection.cs
    Allows player to select the character they want to play as.

    Reference:
    RumpledCode. "Unity Tutorial - Simple Character Selection System." YouTube.
    https://www.youtube.com/watch?v=3qlRgICRoeA

    Adapted for this game project.

    Audio files:
    characterSelectionMusic
    buttonClickSFX
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;
    public AudioClip characterSelectionMusic;
    public AudioClip buttonClickSFX;

    public void Start() {
        if (SoundManager.Instance != null && characterSelectionMusic != null) {
            SoundManager.Instance.PlayMusic(characterSelectionMusic);
        }
    }

    public void NextCharacter() {
        SoundManager.Instance.Play(buttonClickSFX);
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter() {
        SoundManager.Instance.Play(buttonClickSFX);
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;

        if (selectedCharacter < 0) {
            selectedCharacter += characters.Length;
        }

        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame() {
        SoundManager.Instance.Play(buttonClickSFX);
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene(2);
    }
}
