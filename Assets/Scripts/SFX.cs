using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource playerFlashlight;
    public AudioSource industrialLightKillSound;
    public AudioSource buttonBuzz;
    public AudioSource playerGunShot;
    public AudioSource[] playerSteps;
    public AudioSource playerJump;
    public AudioSource playerLanding;
    public AudioSource playerRocketDash;
    public AudioSource playerPowerUp;
    public AudioSource openInventory;
    public AudioSource closeInventory;
    public AudioSource inventoryClick;
    public AudioSource wind;
    public AudioSource calmAmbience;
    public AudioSource intenseMusic;
    public AudioSource morkoGrowling;
    public AudioSource morkoGrowlOne;
    public AudioSource morkoBreathing;
    public AudioSource morkoNeckCrunch;
    public AudioSource morkoSteps;
    public AudioSource spaceshipLaser;
    public AudioSource spaceshipLaserExplosion;
    public AudioSource rockSlide;
    public AudioSource spaceshipRumbling;
    public AudioSource run;
    public AudioSource titleScreen;

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

    public void PlayIndustrialLightKillSound()
    {
        industrialLightKillSound.Play();
    }

    public void PlayButtonBuzz()
    {
        buttonBuzz.Play();
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

    public void StopPlayerRunningSound()
    {
        for(int i = 0; i < 4;i++)
            playerSteps[i].Stop();
    }

    public void PlayPlayerRocketDash()
    {
        playerRocketDash.Play();
    }

    public void PlayJump()
    {
        playerJump.Play();
    }

    public void PlayLanding()
    {
        playerLanding.Play();
    }

    public void PlayPowerUp()
    {
        playerPowerUp.Play();
    }

    public void PlayOpenInventory()
    {
        openInventory.Play();
    }

    public void PlayCloseInventory()
    {
        closeInventory.Play();
    }

    public void PlayInventoryClick()
    {
        inventoryClick.Play();
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

    public void PlayMorkoGrowling()
    {
        morkoGrowling.Play();
    }

    public void PlayMorkoGrowlOne()
    {
        morkoGrowlOne.Play();
    }

    public void PlayMorkoBreathing()
    {
        morkoBreathing.Play();
    }

    public void PlayMorkoNeckCrunch()
    {
        morkoNeckCrunch.Play();
    }

    public void PlayMorkoSteps()
    {
        morkoSteps.Play();
    }

    public void PlaySpaceshipRumbling()
    {
        spaceshipRumbling.Play();
    }

    public void PlaySpaceshipLaser()
    {
        spaceshipLaser.Play();
    }

    public void PlaySpaceshipLaserExplosion()
    {
        spaceshipLaserExplosion.Play();
    }

    public void PlayRockSlide()
    {
        rockSlide.Play();
    }

    public void PlayTitleScreen()
    {
        titleScreen.Play();
    }
    public void PlayRun()
    {
        run.Play();
    }
}
