using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;
    private GameObject player;

    private AudioSource _music;
    private AudioSource _sfx;
    private AudioSource _musicToFadeOut;
    private AudioSource _musicToFadeIn;
    private AudioSource _sfxToFadeOut;
    private AudioSource _sfxToFadeIn;

    private bool canBeFadedMusic = false;
    private bool canBeFadedSFX = false;
    private bool lowerTheSelectedMusicVolume = false;
    private bool increaseTheSelectedMusicVolume = false;
    private bool lowerTheSelectedSFXVolume = false;
    private bool increaseTheSelectedSFXVolume = false;
    public bool upIsTriggered = false;
    public bool downIsTriggered = false;
    private bool loop = false;
    public bool destroyOnTrigger = false;

    private float musicFadeTime = 4;
    private float sfxFadeTime = 4;
    private float desiredVolume = 0.5f;
    private float desiredIncreasedMusicVolume = 0;
    private float desiredIncreasedSFXVolume = 0;
    private float desiredFadedMusicVolume = 0;
    private float desiredFadedSFXVolume = 0;
    private float startingPitch = 1;
    private float desiredPitch;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
    }

    private void FixedUpdate()
    {
        if (lowerTheSelectedMusicVolume)
            MusicFade();

        if (increaseTheSelectedMusicVolume)
            MusicIncrease();

        if (lowerTheSelectedSFXVolume)
            SFXFade();

        if (increaseTheSelectedSFXVolume)
            SFXIncrease();

        if (downIsTriggered)
            SFXDownPitcher();

        if (upIsTriggered)
            SFXUpPitcher();

        if (canBeFadedMusic)
            FadeOutAndInMusicControl();

        if (canBeFadedSFX)
            FadeOutAndInSFXControl();

    }

    private void MusicIncrease()
    {
        lowerTheSelectedMusicVolume = false;
        if (_musicToFadeIn.volume >= desiredIncreasedMusicVolume)
        {
            increaseTheSelectedMusicVolume = false;
            _musicToFadeIn.volume = desiredIncreasedMusicVolume;
            return;
        }

        _musicToFadeIn.volume += Time.deltaTime * 1 / musicFadeTime;

        if (_musicToFadeIn.volume >= desiredIncreasedMusicVolume)
        {
            _musicToFadeIn.volume = desiredIncreasedMusicVolume;
            increaseTheSelectedMusicVolume = false;
        }
    }

    private void MusicFade()
    {
        increaseTheSelectedMusicVolume = false;
        if (_musicToFadeOut.volume == desiredFadedMusicVolume)
        {
            lowerTheSelectedMusicVolume = false;
            return;
        }

        _musicToFadeOut.volume -= Time.deltaTime * 1 / musicFadeTime;

        if (_musicToFadeOut.volume <= desiredFadedMusicVolume)
        {
            //_audio.Pause();
            _musicToFadeOut.volume = desiredFadedMusicVolume;
            lowerTheSelectedMusicVolume = false;
        }
    }

    private void SFXFade()
    {
        increaseTheSelectedSFXVolume = false;
        if (_sfxToFadeOut.volume == desiredFadedSFXVolume)
        {
            lowerTheSelectedSFXVolume = false;
            return;
        }

        _sfxToFadeOut.volume -= Time.deltaTime * 1 / sfxFadeTime;

        if (_sfxToFadeOut.volume <= desiredFadedSFXVolume)
        {
            //_audio.Pause();
            _sfxToFadeOut.volume = desiredFadedSFXVolume;
            lowerTheSelectedSFXVolume = false;
        }
    }

    private void SFXIncrease()
    {
        lowerTheSelectedSFXVolume = false;
        if (_sfxToFadeIn.volume >= desiredIncreasedSFXVolume)
        {
            increaseTheSelectedSFXVolume = false;
            _sfxToFadeIn.volume = desiredIncreasedSFXVolume;
            return;
        }

        _sfxToFadeIn.volume += Time.deltaTime * 1 / sfxFadeTime;

        if (_sfxToFadeIn.volume >= desiredIncreasedSFXVolume)
        {
            _sfxToFadeIn.volume = desiredIncreasedSFXVolume;
            increaseTheSelectedSFXVolume = false;
        }
    }

    private void SFXDownPitcher()
    {
         _sfx.pitch -= Time.deltaTime * startingPitch / sfxFadeTime;
        if (_sfx.pitch <= desiredPitch - 0.02)
        {
            downIsTriggered = false;
            _sfx.pitch = desiredPitch;
        }

    }

    private void SFXUpPitcher()
    {
        _sfx.pitch += Time.deltaTime * startingPitch / sfxFadeTime;

        if (_sfx.pitch >= desiredPitch + 0.02)
        {
            upIsTriggered = false;
            _sfx.pitch = desiredPitch;
        }
    }

    public void Music(AudioSource audio)
    {
        _music = audio;
    }

    public void SFX(AudioSource audio)
    {
        _sfx = audio;
    }

    public void PlaySFX()
    {
        _sfx.volume = desiredVolume;
        _sfx.loop = loop;
        _sfx.Play();
    }


    public void PlaySelectedSFX()
    {
        if (!_sfx.isPlaying)
        {
            increaseTheSelectedSFXVolume = false;
            lowerTheSelectedSFXVolume = false;
            _sfx.volume = desiredVolume;
            _sfx.Play();
        }
    }

    public void PlaySelectedMusic()
    {
        if (!_music.isPlaying)
        {
            increaseTheSelectedMusicVolume = false;
            lowerTheSelectedMusicVolume = false;
            _audioController.StopAllMusic();
            _music.volume = desiredVolume;
            _music.Play();
        }
    }

    public void MusicToFadeOut(AudioSource fadeOutAudio)
    {
        _musicToFadeOut = fadeOutAudio;
    }

    public void SFXToFadeOut(AudioSource fadeOutAudio)
    {
        _sfxToFadeOut = fadeOutAudio;
    }

    public void MusicToFadeIn(AudioSource fadeInAudio)
    {
        _musicToFadeIn = fadeInAudio;
    }

    public void SFXToFadeIn(AudioSource fadeInAudio)
    {
        _sfxToFadeIn = fadeInAudio;
    }

    public void StopSelectedAudio()
    {
        if(_sfx != null)
             _sfx.Stop();

        if (_music != null)
            _music.Stop();        
    }
  

    //public void StopSelectedMusic()
    //{
    //    _music.Stop();

    //    _audio.Stop();
    //    _audioController.intenseMusic.Stop();
    //}

    //public void FadeOutMusic()
    //{
    //    increaseTheMusicVolume = false;
    //    lowerTheMusicVolume = true;
    //}

    //public void FadeInMusic(float volume)
    //{
    //    desiredVolume = volume;
    //    lowerTheMusicVolume = false;
    //    increaseTheMusicVolume = true;
    //}

    public void FadeOutSelectedMusic(float volume)
    {
        desiredFadedMusicVolume = volume;
        increaseTheSelectedMusicVolume = false;
        lowerTheSelectedMusicVolume = true;   
    }

    public void FadeInSelectedMusic(float volume)
    {
        desiredIncreasedMusicVolume = volume;
        lowerTheSelectedMusicVolume = false;
        increaseTheSelectedMusicVolume = true;
    }

    public void FadeOutSelectedSFX(float volume)
    {
        desiredFadedSFXVolume = volume;
        increaseTheSelectedSFXVolume = false;
        lowerTheSelectedSFXVolume = true;  
    }

    public void FadeInSelectedSFX(float volume, float pitch)
    {
        desiredPitch = pitch;
        desiredIncreasedSFXVolume = volume;
        _sfxToFadeIn.pitch = desiredPitch;
        lowerTheSelectedSFXVolume = false;
        increaseTheSelectedSFXVolume = true;
    }

    public void SFXPitchDown(float desiredSoundPitch)
    {
        desiredPitch = desiredSoundPitch;
        upIsTriggered = false;
        downIsTriggered = true;
    }

    public void SFXPitchUp(float desiredSoundPitch)
    {
        desiredPitch = desiredSoundPitch;
        downIsTriggered = false;
        upIsTriggered = true;
    }

    public void MusicFadeTime(float desiredFadeTime)
    {
        musicFadeTime = desiredFadeTime;
    }

    public void SFXFadeTime(float desiredFadeTime)
    {
        sfxFadeTime = desiredFadeTime;
    }

    public void Volume(float volume)
    {
        desiredVolume = volume;
    }

    public void Pitch(float pitch)
    {
        desiredPitch = pitch;
        _sfx.pitch = desiredPitch;
    }

    public void DesiredIncreasedMusicVolume(float increasedVolume)
    {
        desiredIncreasedMusicVolume = increasedVolume;
    }

    public void DesiredIncreasedSFXVolume(float increasedVolume)
    {
        desiredIncreasedSFXVolume = increasedVolume;
    }

    public void DesiredFadedMusicVolume(float fadedVolume)
    {
        desiredFadedMusicVolume = fadedVolume;
    }

    public void DesiredFadedSFXVolume(float fadedVolume)
    {
        desiredFadedSFXVolume = fadedVolume;
    }

    public void Loop(bool looped)
    {
        loop = looped;
    }

    public void FadeOutAndInMusic()
    {

        if (_musicToFadeIn.isPlaying)
        {
            return;
        }
        canBeFadedMusic = true;
    }
    private void FadeOutAndInMusicControl()
    {
        lowerTheSelectedMusicVolume = true;
        if(_musicToFadeOut.volume == 0 && canBeFadedMusic)
        {
            lowerTheSelectedMusicVolume = false;
            _musicToFadeIn.volume = 0;
            _musicToFadeOut.Stop();
            _musicToFadeIn.Play();

            increaseTheSelectedMusicVolume = true;
            canBeFadedMusic = false;
        }
    }
    public void FadeOutAndInSFX()
    {

        if (_sfxToFadeIn.isPlaying)
        {
            return;
        }
        canBeFadedSFX = true;
    }

    private void FadeOutAndInSFXControl()
    {
        lowerTheSelectedSFXVolume = true;
        if (_sfxToFadeOut.volume == 0 && canBeFadedSFX)
        {
            lowerTheSelectedSFXVolume = false;
            _sfxToFadeIn.volume = 0;
            _sfxToFadeOut.Stop();
            _sfxToFadeIn.Play();

            increaseTheSelectedSFXVolume = true;
            canBeFadedSFX = false;
        }
    }
}
