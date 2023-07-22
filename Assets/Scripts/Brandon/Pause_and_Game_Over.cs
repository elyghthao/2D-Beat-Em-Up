using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_and_Game_Over : MonoBehaviour
{
    private static bool game_paused;
    private static bool game_over;
    private GameObject enemies;
    private GameObject player;
    private GameObject bgm_object;
    private GameObject black_screen;
    private Scene current_scene;

    // Start is called before the first frame update
    void Start()
    {
        game_paused = false;
        game_over = false;

        enemies = GameObject.Find("Enemies");
        player = GameObject.Find("Player");
        bgm_object = GameObject.Find("BGM");
        black_screen = gameObject.transform.GetChild(0).gameObject;
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

        bgm_object.GetComponent<Background_Music>().PlayPauseSound();
        black_screen.SetActive(true);

        Time.timeScale = 0f;
        //player.SetActive(false);
        player.GetComponent<PlayerStateMachine>().enabled = false;
        enemies.SetActive(false);
    }

    void ResumeGame()
    {
        game_paused = false;

        bgm_object.GetComponent<Background_Music>().PlayBackgroundMusic();
        black_screen.SetActive(false);

        Time.timeScale = 1f;
        //player.SetActive(true);
        player.GetComponent<PlayerStateMachine>().enabled = true;
        enemies.SetActive(true);
    }

    void GameOver()
    {
        game_over = true;

        bgm_object.GetComponent<Background_Music>().PlayGameOverSound();
        black_screen.SetActive(true);

        Time.timeScale = 0f;
        //player.SetActive(false);
        player.GetComponent<PlayerStateMachine>().enabled = false;
        enemies.SetActive(false);
    }
}
