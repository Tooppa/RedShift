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
    public float timeToDecrease;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
        musicFader = GameObject.Find("MusicFader");
        musicIncreaser = GameObject.Find("MusicIncreaser");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            switch(musicTrigger)
            {
                case "Calm":
                    musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
                    musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
                    _audioController.GetComponent<SFX>().intenseMusic.Stop();
                    _audioController.GetComponent<SFX>().PlayCalmAmbience();
                    if(destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case "Intense":
                    musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
                    musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
                    _audioController.GetComponent<SFX>().calmAmbience.Pause();
                    _audioController.GetComponent<SFX>().intenseMusic.volume = 0.35f;
                    _audioController.GetComponent<SFX>().PlayIntenseMusic();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case "FadeMusic":
                    musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
                    musicFader.GetComponent<MusicFader>().lowerTheVolume = true;
                    break;

                case "IncreaseMusic":
                    musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
                    musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = true;
                    break;

                case "StopMusic":
                    _audioController.GetComponent<SFX>().calmAmbience.Stop();
                    _audioController.GetComponent<SFX>().intenseMusic.Stop();
                    _audioController.GetComponent<SFX>().calmAmbience.volume = 0.35f;
                    _audioController.GetComponent<SFX>().intenseMusic.volume = 0.35f;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;
            }
        }
    }
}
