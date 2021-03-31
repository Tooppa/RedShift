using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchChangerDown : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;
    private GameObject pitchChangerUp;

    public bool downIsTriggered = false;

    public float startingPitch = 1;
    public float endingPitch = 0.35f;
    public float timeToDecrease = 5;

    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
        pitchChangerUp = GameObject.Find("WindPitchChangerUp");

    }

    private void FixedUpdate()
    {
        if (downIsTriggered)
        {
            _audioController.GetComponent<SFX>().wind.pitch -= Time.deltaTime * startingPitch / timeToDecrease;
        }
        if (downIsTriggered && _audioController.GetComponent<SFX>().wind.pitch <= endingPitch + 0.02)
        {
            downIsTriggered = false;
            _audioController.GetComponent<SFX>().wind.pitch = 0.35f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && _audioController.GetComponent<SFX>().wind.pitch <= startingPitch)
        {
            if (pitchChangerUp.GetComponent<PitchChangerUp>().upIsTriggered == true)
                pitchChangerUp.GetComponent<PitchChangerUp>().upIsTriggered = false;

            downIsTriggered = true;
        }
    }
}
