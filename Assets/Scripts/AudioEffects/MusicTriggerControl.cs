using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;

    private AudioSource _audio;

    private float fadeTime = 4;

    private string _audioName;

    private bool canBeFaded = false;
    private bool lowerTheVolume = false;
    private bool increaseTheVolume = false;
    private bool upIsTriggered = false;
    private bool downIsTriggered = false;

    private float desiredVolume = 0.5f;
    private bool destroyOnTrigger = false;

    private float startingPitch = 1;
    private float desiredPitch;

    private float waitTimeBetweenTransition;

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

        if (canBeFaded)
            FadeOutAndInControl(_audioName);
    }

    private void MusicIncrease()
    {
        lowerTheVolume = false;
        if (_audioController.calmAmbience.volume >= desiredVolume && _audioController.intenseMusic.volume >= desiredVolume)
        {
            increaseTheVolume = false;
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.intenseMusic.volume = desiredVolume;
            return;
        }

        _audioController.calmAmbience.volume += Time.deltaTime * 1 / fadeTime;
        _audioController.intenseMusic.volume += Time.deltaTime * 1 / fadeTime;

        if (_audioController.calmAmbience.volume >= desiredVolume && _audioController.intenseMusic.volume >= desiredVolume)
        {
            _audioController.calmAmbience.volume = desiredVolume;
            _audioController.intenseMusic.volume = desiredVolume;

            increaseTheVolume = false;
        }
    }

    private void MusicFade()
    {
        increaseTheVolume = false;
        if (_audioController.calmAmbience.volume == 0 && _audioController.intenseMusic.volume == 0)
        {
            lowerTheVolume = false;
            return;
        }

        _audioController.calmAmbience.volume -= Time.deltaTime * 1 / fadeTime;
        _audioController.intenseMusic.volume -= Time.deltaTime * 1 / fadeTime;

        if (_audioController.calmAmbience.volume == 0 && _audioController.intenseMusic.volume == 0)
        {
            //_audio.Pause();
            lowerTheVolume = false;
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }

    private void DownPitcher()
    {
        _audio.pitch -= Time.deltaTime * startingPitch / fadeTime;
        if (_audio.pitch <= desiredPitch - 0.02)
        {
            downIsTriggered = false;
            _audio.pitch = desiredPitch;
        }

    }

    private void UpPitcher()
    {
        _audio.pitch += Time.deltaTime * startingPitch / fadeTime;

        if (_audio.pitch >= desiredPitch + 0.02)
        {
            upIsTriggered = false;
            _audio.pitch = desiredPitch;
        }
    }

    public void SelectSoundMusic(AudioSource audio)
    {
        _audio = audio;
    }

    public void PlaySFX(AudioSource audio, bool loop)
    {

        audio.Play();
        if (loop == true)
            audio.loop = true;
    }

    public void CalmStart(float volume)
    {
        if(!_audioController.calmAmbience.isPlaying)
        {
            increaseTheVolume = false;
            lowerTheVolume = false;
            _audioController.StopAllMusic();
            _audioController.calmAmbience.volume = volume;
            _audioController.calmAmbience.Play();
        }
    }

    public void IntenseStart(float volume)
    {
        if(!_audioController.intenseMusic.isPlaying)
        {
            increaseTheVolume = false;
            lowerTheVolume = false;
            _audioController.StopAllMusic();
            _audioController.intenseMusic.volume = volume;
            _audioController.intenseMusic.Play();
        }
    }

    public void StopSelectedMusic()
    {
        _audio.Stop();
        //_audioController.intenseMusic.Stop();
        if (destroyOnTrigger)
            Destroy(gameObject);
    }

    public void FadeOutMusic()
    {
        increaseTheVolume = false;
        lowerTheVolume = true;
    }

    public void FadeInMusic(float volume)
    {
        desiredVolume = volume;
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

    public void FadeTime(float desiredFadeTime)
    {
        fadeTime = desiredFadeTime;
    }

    public void Volume(float volume)
    {
        desiredVolume = volume;
    }

    public void TransitionTime(float waitTime)
    {
        waitTimeBetweenTransition = waitTime;
    }

    //public void FadeOutAndIn(string audioName)
    //{
    //    _audioName = audioName;
    //    fadeOutIn = true;
    //}

    //private void FadeOutAndInSong()
    //{
    //    if((_audioController.calmAmbience.isPlaying && _audioName == "Calm") || _audioController.intenseMusic.isPlaying && _audioName == "Intense")
    //    {
    //        return;
    //    }

    //    switch (caseNumber)
    //    {
    //        case 1:
    //            if (_audio.volume > 0)
    //            {
    //                lowerTheVolume = true;
    //            }
    //            if (_audio.volume == 0)
    //            {
    //                lowerTheVolume = false;
                    
    //                if (_audioName == "Intense")
    //                {
    //                    _audioController.intenseMusic.volume = 0;
    //                    _audioController.calmAmbience.Stop();
    //                    _audioController.intenseMusic.Play();
    //                }
    //                if (_audioName == "Calm")
    //                {
    //                    _audioController.calmAmbience.volume = 0;
    //                    _audioController.intenseMusic.Stop();
    //                    _audioController.calmAmbience.Play();
    //                }
    //                caseNumber = 2;
    //            }
    //            break;

    //        case 2:
    //            if(_audio.volume <= desiredVolume)
    //            {
    //                increaseTheVolume = true;
    //            }
    //            if(_audio.volume > desiredVolume)
    //            {
    //                increaseTheVolume = false;
    //                fadeOutIn = false;
                    
    //            }
    //            break;
    //    }
    //    if (!fadeOutIn)
    //        caseNumber = 1;
    //    //StartCoroutine(FadeOutAndInCR(audioName));
    //}

    public void FadeOutAndInMusic(string audioName)
    {
        _audioName = audioName;

        if (_audioController.CurrentMusicPlaying() == audioName)
        {
            return;
        }
        canBeFaded = true;
    }

    private void FadeOutAndInControl(string audioName)
    {
        lowerTheVolume = true;
        if(_audioController.intenseMusic.volume == 0 && _audioController.calmAmbience.volume == 0 && canBeFaded)
        {
            lowerTheVolume = false;
            
            if (audioName == "Calm")
            {
                _audioController.StopAllMusic();
                _audioController.calmAmbience.Play();
            }
            if (audioName == "Intense")
            {
                _audioController.StopAllMusic();
                _audioController.intenseMusic.Play();
            }
            increaseTheVolume = true;
            canBeFaded = false;

        }
    }
}
