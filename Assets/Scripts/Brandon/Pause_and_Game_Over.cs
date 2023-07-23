using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Pause_and_Game_Over : MonoBehaviour
{
    private static bool game_paused; // Controls whether the game is currently paused.
    private static bool game_over; // Controls whether the game is currently in a game over state.
    private GameManager game_manager; // The game manager to access the enemy state machine scripts.
    private GameObject player; // The player's game object.
    private GameObject bgm_object; // The backgroud music game object.
    private GameObject black_screen; // The game object for the transparent black screen.
    private GameObject pause_text_display; // The game object for the text displaying the pause/game over screen.
    private GameObject controls_text_display; // The game object for the text displaying the pause/game over screen controls.
    private Scene current_scene; // The currently loaded scene.
    private List<EnemyStateMachine> enemy_state_machine_scripts; // The list of all of the enemies' state machine scripts.

    // Start is called before the first frame update
    void Start()
    {
        // The level starts unpaused and un-game-overed.
        game_paused = false;
        game_over = false;

        // The game manager object is stored.
        game_manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        
        // The player and background music objects are stored.
        player = GameObject.Find("Player");
        bgm_object = GameObject.Find("BGM");

        // The black screen and pause/game over screen text objects are stored.
        black_screen = gameObject.transform.GetChild(0).gameObject;
        pause_text_display = gameObject.transform.GetChild(1).gameObject;
        controls_text_display = gameObject.transform.GetChild(2).gameObject;
        
        // The current scene is stored.
        current_scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        // If the "enter" key is pressed and the game is not over...
        if (Input.GetKeyDown(KeyCode.Return) && game_over == false)
        {
            // If the game is not paused, then it is paused. Otherwise,
            // the game is resumed from being paused.
            if (game_paused == false)
            {
                PauseGame();
            }
            else if (game_paused == true)
            {
                ResumeGame();
            }
        }

        // If the "r" key is pressed and the game is currently paused or
        // in a game over, then the current scene restarts.
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene(current_scene.name);
            }
        }

        // If the "y" key is pressed and the game is currently paused or
        // in a game over, then the game returns to the title screen.
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }
        
        // If the player runs out of health and the game is not currently in a game over
        // state, then the game is over.
        if (player.GetComponent<PlayerStateMachine>().CurrentHealth <= 0 && game_over == false)
        {
            GameOver();
        }
    }

    void PauseGame()
    {
        // Th game is paused.
        game_paused = true;

        // The pause screen music starts playing, and the transparent black screen is activated.
        bgm_object.GetComponent<Background_Music>().PlayPauseMusic();
        black_screen.SetActive(true);

        // The pause screen text and controls text are activated.
        pause_text_display.SetActive(true);
        controls_text_display.SetActive(true);

        // The associated text for the pause screen is displayed.
        pause_text_display.GetComponent<TMP_Text>().text = "PAUSED";
        controls_text_display.GetComponent<TMP_Text>().text =
        "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen\n" + "\"Enter\": Resume the game";

        // The timescale is set to 0, and the state machine scripts for the player and all of the
        // enemies are disabled.
        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach (EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }

    void ResumeGame()
    {
        // The game is resumed, or unpaused.
        game_paused = false;

        // The level music starts playing, and the transparent black screen is deactivated.
        bgm_object.GetComponent<Background_Music>().PlayBackgroundMusic();
        black_screen.SetActive(false);

        // The pause screen text and controls text are deactivated.
        pause_text_display.SetActive(false);
        controls_text_display.SetActive(false);

        // The timescale is set to 1, and the state machine scripts for the player and all of the
        // enemies are re-enabled.
        Time.timeScale = 1f;
        player.GetComponent<PlayerStateMachine>().enabled = true;
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach (EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = true;
        }
    }

    void GameOver()
    {
        // The game is over.
        game_over = true;

        // The game over scound effect plays, and the transparent black screen is activated.
        bgm_object.GetComponent<Background_Music>().PlayGameOverSound();
        black_screen.SetActive(true);

        // The game over screen and controls text are activated.
        pause_text_display.SetActive(true);
        controls_text_display.SetActive(true);

        // The associated text for the game over screen is displayed.
        pause_text_display.GetComponent<TMP_Text>().text = "GAME OVER";
        controls_text_display.GetComponent<TMP_Text>().text =
        "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen";

        // The timescale is set to 0, and the state machine scripts for the player and all of the
        // enemies are disabled.
        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach (EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }
}
