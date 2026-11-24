/* SoundManager.cs
    Manages background music and SFX.

    Reference: 
    Daggerhart Lab. "Unity: Audio and Sound Manager Singleton Script."
    https://www.daggerhartlab.com/unity-audio-and-sound-manager-singleton-script/

    Adapted for this game project.

    @Janice:
        mainMenuMusic
        winScreenMusic
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    // Audio player components
    public AudioSource SFXSource;
    public AudioSource MusicSource;

    // Singleton Instance
    public static SoundManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }

        // Prevents sound source from auto-destroying when scene is reloaded
        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the SFX source.
    public void Play(AudioClip clip) {
        if (SFXSource != null && clip != null) {
            SFXSource.PlayOneShot(clip);
        }
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip) {
        if (MusicSource == null || clip == null) {
            return;
        }

        if (MusicSource.clip == clip && MusicSource.isPlaying) {
            return;
        }

        MusicSource.clip = clip;
        MusicSource.Play();
    }
}