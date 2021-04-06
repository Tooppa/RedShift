using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource playerFlashlight;
    public AudioSource playerGunShot;
    public AudioSource playerRunning;
    public AudioSource playerJump;
    public AudioSource playerLanding;
    public AudioSource wind;
    public AudioSource calmAmbience;
    public AudioSource intenseMusic;

    public void PlayClick()
    {
        playerFlashlight.Play();
    }

    public void PlayGunShot()
    {
        playerGunShot.Play();
    }

    public void PlayRunning()
    {
        playerRunning.Play();
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
}
