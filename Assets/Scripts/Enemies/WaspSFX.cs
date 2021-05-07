using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSFX : MonoBehaviour
{
    public AudioSource waspBuzz;
    public AudioSource waspDeath;
    public AudioSource waspAttack;

    public void PlayWaspBuzz()
    {
        waspBuzz.Play();
    }

    public void PlayWaspAttack()
    {
        waspAttack.Play();
    }

    public void PlayWaspDeath()
    {
        waspDeath.Play();
    }

}
