using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static bool gamePaused;  // Bool vaLue that determines if the game is paused or not
    public GameObject pauseMenu;    // Menu of the pause menu
    public GameObject player;       // Player

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
}
