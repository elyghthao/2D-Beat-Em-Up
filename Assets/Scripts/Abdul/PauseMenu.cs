using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour {
    public static bool gamePaused;  // Bool vaLue that determines if the game is paused or not
    public GameObject pauseMenu;    // Menu of the pause menu
    public GameObject player;       // Player
    public Slider musicSlider;      // Music Slider
    public AudioMixer musicMixer;   // Music Mixer

    // Start is called before the first frame update
    void Start() {
        gamePaused = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseGame();
        }
    }

    // Pauses/Resumes the game
    void pauseGame() {
        if (gamePaused) {
            resumeGame();
        } else {
            Time.timeScale = 0f;
            gamePaused = true;
            player.SetActive(false);
            if (pauseMenu) {
                pauseMenu.SetActive(true);
            }
        }
    }

    // Resume the game
    public void resumeGame() {
        Time.timeScale = 1f;
        gamePaused = false;
        player.SetActive(true);
        if (pauseMenu) {
            pauseMenu.SetActive(false);
        }
    }

    // When the quit button is pressed, quit the game
    public void quitPressed() {
        Application.Quit();
    }

    // Updates Music Values
    public void MusicChangeSlider(float value) {
        
    }
}
