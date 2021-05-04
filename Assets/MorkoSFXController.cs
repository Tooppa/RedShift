using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorkoSFXController : MonoBehaviour
{
    public AudioSource[] morkoSteps;
    public AudioSource morkoScream;
    public AudioSource morkoGrowl;
    public AudioSource morkoLaugh;

    public void PlayRandomMorkoStep()
    {
        int random = Random.Range(0, 3);
        morkoSteps[random].Play();
    }

    public void PlayMorkoScream()
    {
        morkoScream.Play();
    }

    public void PlayMorkoGrowl()
    {
        morkoGrowl.Play();
    }

    public void PlayMorkoLaugh()
    {
        morkoLaugh.Play();
    }
}
