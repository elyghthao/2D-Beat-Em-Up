using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Cursor.visible = true;
    }
    
    // When the play button is pressed, go to scene 1 (level 0)
    public void playPressed() {
        SceneManager.LoadScene("Level_1");
    }

    // When the quit button is pressed, quit the game
    public void quitPressed() {
        Application.Quit();
    }

    public void creditsPressed() {
        SceneManager.LoadScene("Credits");
    }

    // !!! EDITED BY BRANDON ============================================================= !!!
    public void cheatsPressed() {
        SceneManager.LoadScene("Cheats_Screen");
    }

    public void winPressed() {
        SceneManager.LoadScene("Win_Screen");
    }
    // !!! =============================================================================== !!!
}