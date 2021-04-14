using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource playerFlashlight;
    public AudioSource playerGunShot;
    public AudioSource[] playerSteps;
    public AudioSource playerJump;
    public AudioSource playerLanding;
    public AudioSource wind;
    public AudioSource calmAmbience;
    public AudioSource intenseMusic;
    public AudioSource morkoGrowl1;
    public AudioSource morkoGrowl2;

    public string CurrentMusicPlaying()
    {
        for(int i = 0;i < transform.GetChild(1).childCount; i++)
        {
            if (transform.GetChild(1).GetChild(i).GetComponent<AudioSource>().isPlaying)
            {
                if (transform.GetChild(1).GetChild(i).GetComponent<AudioSource>() == calmAmbience)
                    return "Calm";

                if (transform.GetChild(1).GetChild(i).GetComponent<AudioSource>() == intenseMusic)
                    return "Intense";

            }
        }
        return "";
    }

    public bool IsAmbiencePlaying()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).GetComponent<AudioSource>().isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    public void StopAllMusic()
    {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            if (transform.GetChild(1).GetChild(i).GetComponent<AudioSource>().isPlaying)
            {
                transform.GetChild(1).GetChild(i).GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void StopAllAudio()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).GetComponent<AudioSource>().isPlaying)
            {
                transform.GetChild(0).GetChild(i).GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void PlayClick()
    {
        playerFlashlight.Play();
    }

    public void PlayGunShot()
    {
        playerGunShot.Play();
    }

    public void PlayRandomPlayerStepSound()
    {
        int random = Random.Range(0, 3);
        playerSteps[random].Play();
    }

    public void PlayJump()
    {
        playerJump.Play();
    }

    public void PlayLanding()
    {
        playerLanding.Play();
    }

    public void PlayWind()
    {
        wind.Play();
    }

    public void PlayCalmAmbience()
    {
        calmAmbience.Play();
    }

    public void PlayIntenseMusic()
    {
        intenseMusic.Play();
    }

    public void PlayMorkoGrowlOne()
    {
        morkoGrowl1.Play();
    }

    public void PlayMorkoGrowlTwo()
    {
        morkoGrowl2.Play();
    }
}
