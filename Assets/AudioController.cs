using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script:
 * Manages the sound effects in the game (except pronunciation which is controlled by MazeDataController as it concerns loading of data)
 * win sound is played when the player steps on the correct tile and fall sound is played when the player or the goose steps on the wrong tile
 * PlaySound() will be called by MazeTileController
 */
public class AudioController : MonoBehaviour {

    public AudioClip winSound;
    public AudioClip fallSound;
    AudioSource ogms;
	// Use this for initialization
	void Start () {
        ogms = GetComponent<AudioSource>();
        ogms.volume = 0.7f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(bool win)
    {
        if (win == true)
        {
            ogms.clip = winSound;
            ogms.loop = false;
            ogms.Play();
        }
        else
        {
            ogms.clip = fallSound;
            ogms.loop = false;
            ogms.Play();
        }
    }

    public void PlayPronunciation(AudioClip audio_clip)
    {
        ogms.clip = audio_clip;
        ogms.loop = false;
        ogms.Play();
    }
}
