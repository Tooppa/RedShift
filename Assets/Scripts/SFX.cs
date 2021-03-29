using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource click;
    public AudioSource wind;
    public AudioSource calmAmbience;

    public void PlayClick()
    {
        click.Play();
    }

    public void PlayWind()
    {
        wind.Play();
    }

    public void PlayCalmAmbience()
    {
        calmAmbience.Play();
    }
}
