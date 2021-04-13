using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventSystem : MonoBehaviour
{
    private SFX _audioController;
    private GameObject player;
    //public SoundSourceScriptable soundSourceData;

    public AudioSource _sfx;
    public AudioSource _sfxToFadeOut;
    public AudioSource _sfxToFadeIn;

    public AudioOptions audioOptions = new AudioOptions();

    private bool canBeFadedSFX = false;

    private bool lowerTheSelectedSFXVolume = false;
    private bool increaseTheSelectedSFXVolume = false;
    public bool upIsTriggered = false;
    public bool downIsTriggered = false;
    private bool loop = false;
    public bool destroyOnTrigger = false;

    public float sfxFadeTime = 4;
    public float desiredVolume = 0.5f;
    public float desiredIncreasedSFXVolume = 0;
    public float desiredFadedSFXVolume = 0;
    private float startingPitch = 1;
    public float desiredPitch;

    private void Awake()
    {
        //instantiableAudio = soundSourceData.audio;
        //soundSourceLocation = soundSourceData.location;
        //soundSourceLoop = soundSourceData.loop;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
    }

    private void FixedUpdate()
    {

        if (lowerTheSelectedSFXVolume)
            SFXFade();

        if (increaseTheSelectedSFXVolume)
            SFXIncrease();

        if (downIsTriggered)
            SFXDownPitcher();

        if (upIsTriggered)
            SFXUpPitcher();


        if (canBeFadedSFX)
            FadeOutAndInControl();

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

    //public void SelectAudio(AudioSource audio)
    //{
    //    _audio = audio;
    //}

    //public void Music(AudioSource audio)
    //{
    //    _music = audio;
    //}

    public void SFX(AudioSource audio)
    {
        _sfx = audio;
    }

    public void PlaySFX()
    {
        _sfx.Play();
    }


    public void PlaySelectedSFX()
    {
        if (!_sfx.isPlaying)
        {
            increaseTheSelectedSFXVolume = false;
            lowerTheSelectedSFXVolume = false;
            _sfx.volume = desiredVolume;
            //_audioController.StopAllMusic();
            _sfx.Play();
        }
    }



    public void SFXToFadeOut(AudioSource fadeOutAudio)
    {
        _sfxToFadeOut = fadeOutAudio;
    }


    public void SFXToFadeIn(AudioSource fadeInAudio)
    {
        _sfxToFadeIn = fadeInAudio;
    }



    public void StopSelectedAudio()
    {
        if (_sfx != null)
            _sfx.Stop();
        //_audio.Stop();
        //_audioController.intenseMusic.Stop();
    }



    public void FadeOutSelectedSFX(float volume)
    {
        desiredFadedSFXVolume = volume;
        increaseTheSelectedSFXVolume = false;
        lowerTheSelectedSFXVolume = true;
        //increaseTheSelectedSoundVolume = false;
        //lowerTheSelectedSoundVolume = true;    
    }

    public void FadeInSelectedSFX(float volume)
    {
        desiredIncreasedSFXVolume = volume;
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


    public void SFXFadeTime(float desiredFadeTime)
    {
        sfxFadeTime = desiredFadeTime;
    }

    public void Volume(float volume)
    {
        desiredVolume = volume;
    }


    public void DesiredIncreasedSFXVolume(float increasedVolume)
    {
        desiredIncreasedSFXVolume = increasedVolume;
    }


    public void DesiredFadedSFXVolume(float fadedVolume)
    {
        desiredFadedSFXVolume = fadedVolume;
    }

    public void Loop(bool looped)
    {
        loop = looped;
    }


    public void FadeOutAndIn()
    {

        if (_sfxToFadeIn.isPlaying)
        {
            return;
        }
        canBeFadedSFX = true;
    }

    private void FadeOutAndInControl()
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            switch (audioOptions)
            {
                case AudioOptions.Start:

                    upIsTriggered = false;
                    downIsTriggered = false;
                    if (!_sfx.isPlaying)
                    {

                        increaseTheSelectedSFXVolume = false;
                        lowerTheSelectedSFXVolume = false;
                        _audioController.StopAllMusic();
                        _sfx.volume = desiredVolume;
                        _sfx.Play();
                        if (destroyOnTrigger)
                            Destroy(gameObject);
                    }
                    break;

                case AudioOptions.Stop:

                    _sfx.Stop();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOut:

                    upIsTriggered = false;
                    downIsTriggered = false;
                    increaseTheSelectedSFXVolume = false;
                    lowerTheSelectedSFXVolume = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeIn:
                    upIsTriggered = false;
                    downIsTriggered = false;
                    lowerTheSelectedSFXVolume = false;
                    increaseTheSelectedSFXVolume = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOutAndIn:

                    if (_sfxToFadeIn.isPlaying)
                    {
                        break;
                    }

                    canBeFadedSFX = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchUp:
                    downIsTriggered = false;
                    upIsTriggered = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchDown:
                    upIsTriggered = false;
                    downIsTriggered = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

            }
        }
    }
    public enum AudioOptions
    {
        Start,
        Stop,
        FadeOut,
        FadeIn,
        FadeOutAndIn,
        PitchUp,
        PitchDown
    };
}
