using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFader : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject musicFader;

    public float timeToDecrease;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    public bool lowerTheVolume = false;
    public bool increaseTheVolume = false;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        musicFader = GameObject.Find("MusicFader");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (lowerTheVolume)
        {
            MusicFade();
        }
    }

    private void MusicFade()
    {
        if (_audioController.GetComponent<SFX>().calmAmbience.volume == desiredVolume || _audioController.GetComponent<SFX>().intenseMusic.volume == desiredVolume)
        {
            musicFader.GetComponent<MusicFader>().lowerTheVolume = false;
            _audioController.GetComponent<SFX>().calmAmbience.volume = desiredVolume;
            _audioController.GetComponent<SFX>().intenseMusic.volume = desiredVolume;
            lowerTheVolume = false;
            return;
        }
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
}
