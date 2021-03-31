using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;

    public string musicTrigger;
    private float startingVolume;
    public float timeToDecrease;
    public float desiredVolume;

    private bool lowerTheVolume = false;
    private bool increaseTheVolume = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (lowerTheVolume)
            Debug.Log("lowerTrue");
            MusicFader();

        if (increaseTheVolume)
            Debug.Log("incrTrue");
            MusicIncreaser();
    }

    private void MusicFader()
    {
        _audioController.GetComponent<SFX>().calmAmbience.volume -= Time.deltaTime * 1 / timeToDecrease;
        _audioController.GetComponent<SFX>().intenseMusic.volume -= Time.deltaTime * 1 / timeToDecrease;

        if (_audioController.GetComponent<SFX>().calmAmbience.volume == 0 && _audioController.GetComponent<SFX>().intenseMusic.volume == 0)
        {
            //_audioController.GetComponent<SFX>().calmAmbience.Pause();
            //_audioController.GetComponent<SFX>().intenseMusic.Pause();
            lowerTheVolume = false;
        }

    }

    private void MusicIncreaser()
    {
        _audioController.GetComponent<SFX>().calmAmbience.volume += Time.deltaTime * 1 / timeToDecrease;
        _audioController.GetComponent<SFX>().intenseMusic.volume += Time.deltaTime * 1 / timeToDecrease;

        if (_audioController.GetComponent<SFX>().calmAmbience.volume >= desiredVolume && _audioController.GetComponent<SFX>().intenseMusic.volume >= desiredVolume)
        {
            _audioController.GetComponent<SFX>().calmAmbience.volume = desiredVolume;
            _audioController.GetComponent<SFX>().intenseMusic.volume = desiredVolume;
            increaseTheVolume = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            lowerTheVolume = false;
            increaseTheVolume = false;
            switch(musicTrigger)
            {
                case "Calm":
                    _audioController.GetComponent<SFX>().intenseMusic.Stop();
                    _audioController.GetComponent<SFX>().PlayCalmAmbience();
                    Destroy(gameObject);
                    break;

                case "Intense":
                    _audioController.GetComponent<SFX>().calmAmbience.Stop();
                    _audioController.GetComponent<SFX>().PlayIntenseMusic();
                    break;

                case "FadeMusic":
                    lowerTheVolume = true;
                    increaseTheVolume = false;
                    break;

                case "IncreaseMusic":
                    increaseTheVolume = true;
                    lowerTheVolume = false;
                    break;

                case "StopMusic":
                    _audioController.GetComponent<SFX>().calmAmbience.volume = 0;
                    _audioController.GetComponent<SFX>().intenseMusic.volume = 0;
                    break;
            }
        }
    }
}
