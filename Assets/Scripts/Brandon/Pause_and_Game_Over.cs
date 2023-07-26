using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pause_and_Game_Over : MonoBehaviour
{
    private static bool game_paused;
    private static bool game_over;
    private GameObject bgm_object;
    private GameObject player;
    private GameManager game_manager;
    private GameObject black_screen;
    private GameObject pause_text;
    private GameObject controls_text;
    private Scene current_scene;
    private List<EnemyStateMachine> enemy_state_machine_scripts;

    // Start is called before the first frame update
    void Start()
    {
        game_paused = false;
        game_over = false;

        bgm_object = GameObject.Find("BGM");
        player = GameObject.Find("Player");
        game_manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>(); 

        black_screen = gameObject.transform.GetChild(0).gameObject;
        pause_text = gameObject.transform.GetChild(1).gameObject;
        controls_text = gameObject.transform.GetChild(2).gameObject;
        
        current_scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && game_over == false)
        {
            if (game_paused == false)
            {
                PauseGame();
            }
            else if (game_paused == true)
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene(current_scene.name);
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (game_paused == true || game_over == true)
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }
        
        if (player.GetComponent<PlayerStateMachine>().CurrentHealth <= 0 && game_over == false)
        {
            GameOver();   
        }
    }

    void PauseGame()
    {
        game_paused = true;

        bgm_object.GetComponent<Background_Music>().PlayPauseMusic();
        black_screen.SetActive(true);

        pause_text.SetActive(true);
        pause_text.GetComponent<TMP_Text>().text = "PAUSED";

        controls_text.SetActive(true);
        controls_text.GetComponent<TMP_Text>().text =
            "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen\n" + "\"Enter\": Resume the game";

        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;

        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }

    void ResumeGame()
    {
        game_paused = false;

        bgm_object.GetComponent<Background_Music>().PlayBackgroundMusic();
        black_screen.SetActive(false);

        pause_text.SetActive(false);
        pause_text.GetComponent<TMP_Text>().text = "NOT PAUSED";

        controls_text.SetActive(false);
        controls_text.GetComponent<TMP_Text>().text =
            "You shouldn't be able to see this.";

        Time.timeScale = 1f;
        player.GetComponent<PlayerStateMachine>().enabled = true;

        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = true;
        }
    }

    void GameOver()
    {
        game_over = true;

        bgm_object.GetComponent<Background_Music>().PlayGameOverSound();
        black_screen.SetActive(true);

        pause_text.SetActive(true);
        pause_text.GetComponent<TMP_Text>().text = "GAME OVER";

        controls_text.SetActive(true);
        controls_text.GetComponent<TMP_Text>().text =
            "\"R\": Restart the level\n" + "\"Y\": Quit to the title screen\n";

        Time.timeScale = 0f;
        player.GetComponent<PlayerStateMachine>().enabled = false;

        enemy_state_machine_scripts = game_manager.EnemyReferences;

        foreach(EnemyStateMachine esm in enemy_state_machine_scripts)
        {
            esm.enabled = false;
        }
    }

    private void OnDestroy()
    {
        // When this script is destroyed, reset the timescale to 1.
        Time.timeScale = 1f;
    }
}
