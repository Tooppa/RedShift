using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarSFX : MonoBehaviour
{
    public AudioSource attack;
    public AudioSource idle;
    public AudioSource death;

    public void PlayAttack()
    {
        attack.Play();
    }

    public void PlayIdle()
    {
        idle.Play();
    }

    public void PlayDeath()
    {
        death.Play();
    }
}
