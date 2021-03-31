using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchChangerUp : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;
    private GameObject pitchChangerDown;

    public bool upIsTriggered = false;

    public float startingPitch = 1;
    public float endingPitch = 0.35f;
    public float timeToDecrease = 5;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
        pitchChangerDown = GameObject.Find("WindPitchChangerDown");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (upIsTriggered)
        {
            _audioController.GetComponent<SFX>().wind.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        }
        if (upIsTriggered && _audioController.GetComponent<SFX>().wind.pitch >= startingPitch - 0.02)
        {
            upIsTriggered = false;
            _audioController.GetComponent<SFX>().wind.pitch = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && _audioController.GetComponent<SFX>().wind.pitch >= endingPitch)
        {
            if (pitchChangerDown.GetComponent<PitchChangerDown>().downIsTriggered)
                pitchChangerDown.GetComponent<PitchChangerDown>().downIsTriggered = false;

            upIsTriggered = true;
        }
    }
}
