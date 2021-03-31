using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;
    private GameObject musicFader;
    private GameObject musicIncreaser;

    public string musicTrigger;
    private float startingVolume;
    public float timeToDecrease;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    private bool lowerTheVolume = false;
    private bool increaseTheVolume = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
        musicFader = GameObject.Find("MusicFader");
        musicIncreaser = GameObject.Find("MusicIncreaser");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (lowerTheVolume)
        {
            MusicFader();
        }

        if (increaseTheVolume)
        {
            MusicIncreaser();
        }
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
            if (destroyOnTrigger)
                Destroy(gameObject);
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
            if (destroyOnTrigger)
                Destroy(gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            switch(musicTrigger)
            {
                case "Calm":
                    _audioController.GetComponent<SFX>().intenseMusic.Stop();
                    _audioController.GetComponent<SFX>().PlayCalmAmbience();
                    if(destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case "Intense":
                    _audioController.GetComponent<SFX>().calmAmbience.Stop();
                    _audioController.GetComponent<SFX>().PlayIntenseMusic();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case "FadeMusic":
                    musicIncreaser.GetComponent<MusicTriggerControl>().increaseTheVolume = false;
                    lowerTheVolume = true;
                    increaseTheVolume = false;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case "IncreaseMusic":
                    musicFader.GetComponent<MusicTriggerControl>().lowerTheVolume = false;
                    increaseTheVolume = true;
                    lowerTheVolume = false;

                    break;

                case "StopMusic":
                    _audioController.GetComponent<SFX>().calmAmbience.volume = 0;
                    _audioController.GetComponent<SFX>().intenseMusic.volume = 0;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;
            }
        }
    }
}
