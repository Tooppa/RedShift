using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;

    private AudioSource _audio;

    public float fadeTime;

    private bool lowerTheVolume = false;
    private bool increaseTheVolume = false;
    private bool upIsTriggered = false;
    private bool downIsTriggered = false;

    public string musicTrigger;
    public float desiredVolume;
    public bool destroyOnTrigger = false;

    public float startingPitch = 1;
    public float desiredPitch;
    public float timeToDecrease = 5;

    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
    }

    private void FixedUpdate()
    {
        if (lowerTheVolume)
            MusicFade();

        if (increaseTheVolume)
            MusicIncrease();

        if (downIsTriggered)
            DownPitcher();

        if (upIsTriggered)
            UpPitcher();
    }

    public void SelectSoundMusic(AudioSource audio)
    {
        _audio = audio;
    }

    private void MusicIncrease()
    {
        if (_audio.volume >= desiredVolume)
        {
            increaseTheVolume = false;
            _audio.volume = desiredVolume;
            return;
        }

        _audio.volume += Time.deltaTime * 1 / fadeTime;

        if (_audio.volume >= desiredVolume)
        {
            _audio.volume = desiredVolume;

            increaseTheVolume = false;
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }

    private void MusicFade()
    {
        if (_audio.volume == 0)
        {
            lowerTheVolume = false;
            return;
        }

        _audio.volume -= Time.deltaTime * 1 / fadeTime;

        if (_audio.volume == 0)
        {
            //_audio.Pause();
            lowerTheVolume = false;
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }

    public void CalmStart(float desiredVolume)
    {
        if(!_audioController.calmAmbience.isPlaying)
        {
            increaseTheVolume = false;
            lowerTheVolume = false;
            _audioController.intenseMusic.Stop();
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.calmAmbience.Play();
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }

    public void IntenseStart(float desiredVolume)
    {
        if(!_audioController.intenseMusic.isPlaying)
        {
            increaseTheVolume = false;
            lowerTheVolume = false;
            _audioController.calmAmbience.Pause();
            _audioController.intenseMusic.volume = desiredVolume;
            _audioController.intenseMusic.Play();
            if (destroyOnTrigger)
                Destroy(gameObject);
        }

    }

    public void StopMusic()
    {
        _audio.Stop();
        //_audioController.intenseMusic.Stop();
        if (destroyOnTrigger)
            Destroy(gameObject);
    }

    private void DownPitcher()
    {
        _audio.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        if (_audio.pitch >= desiredPitch - 0.02)
        {
            upIsTriggered = false;
            _audio.pitch = desiredPitch;
        }

    }

    private void UpPitcher()
    { 
        _audio.pitch -= Time.deltaTime * startingPitch / timeToDecrease;

        if (_audio.pitch <= desiredPitch + 0.02)
        {
            _audio.pitch = desiredPitch;
        }
    }

    public void FadeOutMusic()
    {
        increaseTheVolume = false;
        lowerTheVolume = true;
    }

    public void FadeInMusic(float musicVolume)
    {
        desiredVolume = musicVolume;
        lowerTheVolume = false;
        increaseTheVolume = true;
    }

    public void PitchDown(float desiredSoundPitch)
    {
        desiredPitch = desiredSoundPitch;
        upIsTriggered = false;
        downIsTriggered = true;
    }

    public void PitchUp(float desiredSoundPitch)
    {
        desiredPitch = desiredSoundPitch;
        downIsTriggered = false;
        upIsTriggered = true;
    }

    public void FadeOutAndInSong(string audioName)
    {
        if((_audioController.calmAmbience.isPlaying && audioName == "Calm") || _audioController.intenseMusic.isPlaying && audioName == "Intense")
        {
            return;
        }
        StartCoroutine(FadeOutAndInCR(audioName));
    }

    private IEnumerator FadeOutAndInCR(string audioName)
    {

        lowerTheVolume = true;
        yield return new WaitForSeconds(2);
        lowerTheVolume = false;
        if(audioName == "Calm")
        {
            _audioController.intenseMusic.Stop();
            _audioController.calmAmbience.Play();
        }
        if(audioName == "Intense")
        {
            _audioController.calmAmbience.Stop();
            _audioController.intenseMusic.Play();
        }
        increaseTheVolume = true;
        yield return new WaitForSeconds(2);
        increaseTheVolume = false;
    }

}
