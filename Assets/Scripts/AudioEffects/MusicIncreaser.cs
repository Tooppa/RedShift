using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIncreaser : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject musicIncreaser;

    public float timeToDecrease;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    public bool lowerTheVolume = false;
    public bool increaseTheVolume = false;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        musicIncreaser = GameObject.Find("MusicIncreaser");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (increaseTheVolume)
        {
            MusicIncrease();
        }
    }

    private void MusicIncrease()
    {
        if (_audioController.GetComponent<SFX>().calmAmbience.volume >= desiredVolume && _audioController.GetComponent<SFX>().intenseMusic.volume >= desiredVolume)
        {
            musicIncreaser.GetComponent<MusicIncreaser>().increaseTheVolume = false;
            increaseTheVolume = false;
            return;
        }

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
}
