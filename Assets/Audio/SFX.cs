using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource click;
    public float cooldownTime = 1f;

    private bool inCooldown;
    private IEnumerator Cooldown()
    {
        //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }

    public void PlayClick()
    {
        if (!inCooldown)
        {
            click.Play();
            StartCoroutine(Cooldown());
        }
    }
}
