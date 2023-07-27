using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Music : MonoBehaviour
{
    public AudioClip bgm_sound; // The audio clip for the current level's background music.
    public AudioClip pause_sound; // The audio clip for the pause screen music.
    public AudioClip game_over_sound; // The audio clip for the game over sound effect.

    // Start is called before the first frame update
    void Start()
    {
        // The scene starts by playing its associated background music.
        PlayBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayBackgroundMusic()
    {
        // The audio clip is changed to the level background music, which
        // starts playing looped.
        GetComponent<AudioSource>().clip = bgm_sound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    public void PlayPauseMusic()
    {
        // The audio clip is changed to the pause screen music, which
        // starts playing looped.
        GetComponent<AudioSource>().clip = pause_sound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    public void PlayGameOverSound()
    {
        // The audio clip is changed to the game over sound effect,
        // which plays once.
        GetComponent<AudioSource>().clip = game_over_sound;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }
}
