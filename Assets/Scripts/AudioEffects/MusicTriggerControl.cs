using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;
    private GameObject player;
    private GameObject musicFader;
    private GameObject musicIncreaser;

    private bool lowerTheVolume = false;
    private bool increaseTheVolume = false;

    public string musicTrigger;
    public float timeToDecrease;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
        player = GameObject.Find("Player");
        musicFader = GameObject.Find("MusicFader");
        musicIncreaser = GameObject.Find("MusicIncreaser");
    }

    private void Update()
    {
        
    }

    public void MusicIncrease()
    {
        if (_audioController.calmAmbience.volume >= desiredVolume || _audioController.intenseMusic.volume >= desiredVolume)
        {
            musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.intenseMusic.volume = desiredVolume;
            increaseTheVolume = false;
            return;
        }

        _audioController.calmAmbience.volume += Time.deltaTime * 1 / timeToDecrease;
        _audioController.intenseMusic.volume += Time.deltaTime * 1 / timeToDecrease;

        if (_audioController.calmAmbience.volume >= desiredVolume && _audioController.intenseMusic.volume >= desiredVolume)
        {
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.intenseMusic.volume = desiredVolume;
            increaseTheVolume = false;
            if (destroyOnTrigger)
                Destroy(gameObject);

        }

    }

    public void MusicFade()
    {
        if (_audioController.calmAmbience.volume == desiredVolume || _audioController.intenseMusic.volume == desiredVolume)
        {
            musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.intenseMusic.volume = desiredVolume;
            lowerTheVolume = false;
            return;
        }
        _audioController.calmAmbience.volume -= Time.deltaTime * 1 / timeToDecrease;
        _audioController.intenseMusic.volume -= Time.deltaTime * 1 / timeToDecrease;

        if (_audioController.calmAmbience.volume == 0 && _audioController.intenseMusic.volume == 0)
        {
            //_audioController.GetComponent<SFX>().calmAmbience.Pause();
            //_audioController.GetComponent<SFX>().intenseMusic.Pause();
            lowerTheVolume = false;
            if (destroyOnTrigger)
                Destroy(gameObject);
        }

    }

    public void Funtionality(string musicTrigger)
    {

        if (musicTrigger == "Calm")
        {
        musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
        musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
        _audioController.intenseMusic.Stop();
        _audioController.PlayCalmAmbience();
        if (destroyOnTrigger)
            Destroy(gameObject);
        }


        if (musicTrigger == "Intense")
        {
            musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
            musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
            _audioController.calmAmbience.Pause();
            _audioController.intenseMusic.volume = 0.35f;
            _audioController.PlayIntenseMusic();
            if (destroyOnTrigger)
                Destroy(gameObject);
        }

        if (musicTrigger == "FadeMusic")
        {
            musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
            musicFader.GetComponent<MusicFader>().lowerTheVolume = true;
        }

        if (musicTrigger == "IncreaseMusic")
        {
            musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
            musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = true;
        }

        if (musicTrigger == "StopMusic")
        {
            _audioController.calmAmbience.Stop();
            _audioController.intenseMusic.Stop();
            _audioController.calmAmbience.volume = 0.35f;
            _audioController.intenseMusic.volume = 0.35f;
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
        
        
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if(other.gameObject == player)
    //    {
    //        switch(musicTrigger)
    //        {
    //            case "Calm":
    //                musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
    //                musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
    //                _audioController.intenseMusic.Stop();
    //                _audioController.PlayCalmAmbience();
    //                if(destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case "Intense":
    //                musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
    //                musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
    //                _audioController.calmAmbience.Pause();
    //                _audioController.intenseMusic.volume = 0.35f;
    //                _audioController.PlayIntenseMusic();
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case "FadeMusic":
    //                musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
    //                musicFader.GetComponent<MusicFader>().lowerTheVolume = true;
    //                break;

    //            case "IncreaseMusic":
    //                musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
    //                musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = true;
    //                break;

    //            case "StopMusic":
    //                _audioController.calmAmbience.Stop();
    //                _audioController.intenseMusic.Stop();
    //                _audioController.calmAmbience.volume = 0.35f;
    //                _audioController.intenseMusic.volume = 0.35f;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;
    //        }
    //    }
    //}
}
