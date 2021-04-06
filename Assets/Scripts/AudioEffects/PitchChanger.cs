using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchChanger : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;
    private GameObject pitchChangerDown;
    private GameObject pitchChangerUp;

    public AudioSource audioSource;

    private bool upIsTriggered = false;
    private bool downIsTriggered = false;

    public string pitchTrigger;

    public float startingPitch = 1;
    public float desiredPitch;
    public float timeToDecrease = 5;
    // Start is called before the first frame update
    void Start()
    {
        //_audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
        pitchChangerDown = GameObject.Find("PitchChangerDown");
        pitchChangerUp = GameObject.Find("PitchChangerUp");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (upIsTriggered)
        {
            PitchUp();
        }

        if (downIsTriggered)
        {
            PitchDown();
        }
    }

    private void PitchUp()
    {
        downIsTriggered = false;
        audioSource.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        if (audioSource.pitch >= startingPitch - 0.02)
        {
            upIsTriggered = false;
            audioSource.pitch = 1;
        }
    }

    private void PitchDown()
    {
        upIsTriggered = false;
        audioSource.pitch -= Time.deltaTime * startingPitch / timeToDecrease;

        if (audioSource.pitch <= desiredPitch + 0.02)
        {
            downIsTriggered = false;
            audioSource.pitch = 0.35f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            switch (pitchTrigger)
            {
                case "Up":
                    pitchChangerDown.GetComponent<PitchChanger>().downIsTriggered = false;
                    downIsTriggered = false;
                    upIsTriggered = true;
                    break;

                case "Down":

                    pitchChangerUp.GetComponent<PitchChanger>().upIsTriggered = false;
                    upIsTriggered = false;
                    downIsTriggered = true;
                    break;
            }
        }
    }
}

