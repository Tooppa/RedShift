using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource click;

    public void PlayClick()
    {
        click.Play();
    }
}
