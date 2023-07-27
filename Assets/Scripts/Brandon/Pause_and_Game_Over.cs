using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pause_and_Game_Over : MonoBehaviour
{
    private static bool game_paused; // Tracks whether the game is currently paused.
    private static bool game_over; // Tracks whether the game is currently in a game over state.
    private GameObject bgm_object; // The object playing the current level's background music.
    private GameObject player; // The player character.
    private GameManager game_manager; // The game manager object, used to access the enemy scripts.
    private GameObject black_screen; // The transparent black screen that appears when the game is paused.
    private GameObject pause_text; // The heading text that appears when the game is paused.
    private GameObject controls_text; // The controls description text that appears when the game is paused.
    private Scene current_scene; // The currently loaded scene.
    private List<EnemyStateMachine> enemy_state_machine_scripts;

    // Start is called before the first frame update
    void Start()
    {
        // The scene starts in a non-paused, non-game-overed state.
        game_paused = false;
        game_over = false;

        // The background music, player, and game mananger objects are stored.
        bgm_object = GameObject.Find("BGM");
        player = GameObject.Find("Player");
        game_manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>(); 

        // The black screen and pause screen text objects are stored.
        black_screen = gameObject.transform.GetChild(0).gameObject;
        pause_text = gameObject.transform.GetChild(1).gameObject;
        controls_text = gameObject.transform.GetChild(2).gameObject;
        
        // The currently loaded scene is stored.
        current_scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        // If the "Enter" key is pressed, and the game is not over...
        if (Input.GetKeyDown(KeyCode.Return) && game_over == false)
        {
            // ...then the game pauses if it is not currently paused,
            // and resumes if it is currently paused.
            if (game_paused == false)
            {
                PauseGame();
            }
            else if (game_paused == true)
            {
                ResumeGame();
            }
        }

        // If the "R" key is pressed, and the game is paused or in
        // a game over, then the current scene reloads.
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene(current_scene.name);
            }
        }

        // If the "Y" key is pressed, and the game is paused or in
        // a game over, then the Title Screen scene loads.
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }
        
        // If the player's health reaches 0 and the game is not over, then the game is over.
        if (player.GetComponent<PlayerStateMachine>().CurrentHealth <= 0 && game_over == false)
        {
            GameOver();   
        }
    }

    void PauseGame()
    {
        // The game enters a paused state.
        game_paused = true;

        // The background music object starts playing the pause music, and the
        // transparent black screen is activated.
        bgm_object.GetComponent<Background_Music>().PlayPauseMusic();
        black_screen.SetActive(true);

        // The pause screen heading text is activated and changed accordingly.
        pause_text.SetActive(true);
        pause_text.GetComponent<TMP_Text>().text = "PAUSED";

        // The controls description text is activated and changed accordingly.
        controls_text.SetActive(true);
        controls_text.GetComponent<TMP_Text>().text =
            "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen\n" + "\"Enter\": Resume the game";

        // The game freezes movement by setting the timescale to 0, and disabling
        // the player's state machine script.
        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;

        // The state machine scripts of every enemy in the scene are also disabled.
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }

    void ResumeGame()
    {
        // The game exits a paused state.
        game_paused = false;

        // The background music object starts playing the level's background music,
        // and the transparent black screen is deactivated.
        bgm_object.GetComponent<Background_Music>().PlayBackgroundMusic();
        black_screen.SetActive(false);

        // The pause screen heading text is deactivated.
        pause_text.SetActive(false);
        pause_text.GetComponent<TMP_Text>().text = "NOT PAUSED";

        // The controls description text is deactivated.
        controls_text.SetActive(false);
        controls_text.GetComponent<TMP_Text>().text =
            "You shouldn't be able to see this.";

        // The game resumes movement by setting the timescale to 1, and re-enabling
        // the player's state machine script.
        Time.timeScale = 1f;
        player.GetComponent<PlayerStateMachine>().enabled = true;

        // The state machine scripts of every enemy in the scene are also re-enabled.
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = true;
        }
    }

    void GameOver()
    {
        // The game enters a game over state.
        game_over = true;

        // The background music object plays the game over sound effect,
        // and the transparent black screen is activated.
        bgm_object.GetComponent<Background_Music>().PlayGameOverSound();
        black_screen.SetActive(true);

        // The game over screen heading text is activated and changed accordingly.
        pause_text.SetActive(true);
        pause_text.GetComponent<TMP_Text>().text = "GAME OVER";

        // The controls description text is activated and changed accordingly.
        controls_text.SetActive(true);
        controls_text.GetComponent<TMP_Text>().text =
            "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen\n";

        // The game freezes movement by setting the timescale to 0, and disabling
        // the player's state machine script.
        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;

        // The state machine scripts of every enemy in the scene are also disabled.
        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }

    private void OnDestroy()
    {
        // When this script is destroyed, resume movement by reseting the timescale to 1.
        Time.timeScale = 1f;
    }
}
