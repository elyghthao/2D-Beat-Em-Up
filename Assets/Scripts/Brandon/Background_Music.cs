using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Music : MonoBehaviour
{
    public AudioClip bgm_sound; // The clip for the current level's background music.
    public AudioClip pause_sound; // The clip for the pause screen background music.
    public AudioClip game_over_sound; // The clip for the game over screen sound effect.

    // Start is called before the first frame update
    void Start()
    {
        // When the level starts, the level's background music starts playing.
        PlayBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackgroundMusic()
    {
        // The current audio clip is changed to the level background music,
        // which plays looped.
        GetComponent<AudioSource>().clip = bgm_sound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    public void PlayPauseMusic()
    {
        // The current audio clip is changed to the pause screen background music,
        // which plays looped.
        GetComponent<AudioSource>().clip = pause_sound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    public void PlayGameOverSound()
    {
        // The current audio clip is changed to the game over screen sound effect,
        // which plays once.
        GetComponent<AudioSource>().clip = game_over_sound;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }
}
