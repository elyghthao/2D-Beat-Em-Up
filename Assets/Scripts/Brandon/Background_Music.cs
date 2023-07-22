using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Music : MonoBehaviour
{
    public AudioClip bgm_sound;
    public AudioClip pause_sound;
    public AudioClip game_over_sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackgroundMusic()
    {
        GetComponent<AudioSource>().clip = bgm_sound;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    public void PlayPauseSound()
    {
        GetComponent<AudioSource>().clip = pause_sound;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }

    public void PlayGameOverSound()
    {
        GetComponent<AudioSource>().clip = game_over_sound;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
    }
}
