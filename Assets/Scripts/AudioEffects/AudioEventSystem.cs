using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventSystem : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;

    private bool upIsTriggered = false;
    private bool downIsTriggered = false;

    private float startingPitch = 1;
    private float endingPitch = 0.35f;

    public AudioSource _audio;

    public float timeToDecrease = 5;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {

        if (downIsTriggered)
        {
            _audio.pitch -= Time.deltaTime * startingPitch / timeToDecrease;
        }
        if(_audio.pitch <= endingPitch + 0.02)
        {
            downIsTriggered = false;
            _audio.pitch = 0.35f;
        }

        if (upIsTriggered)
        {
            _audio.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        }
        if (_audio.pitch >= startingPitch - 0.02)
        {
            upIsTriggered = false;
            _audio.pitch = 1;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && _audio.pitch == startingPitch)
        {
            downIsTriggered = true;
        }

        if (other.gameObject == player && _audio.pitch == endingPitch)
        {
            upIsTriggered = true;
        }
    }
}
