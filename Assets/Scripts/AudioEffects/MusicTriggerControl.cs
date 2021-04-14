using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;
    private GameObject player;
    //public SoundSourceScriptable soundSourceData;

    private AudioSource _music;
    private AudioSource _sfx;
    private AudioSource _musicToFadeOut;
    private AudioSource _musicToFadeIn;
    private AudioSource _sfxToFadeOut;
    private AudioSource _sfxToFadeIn;


    //private AudioSource instantiableAudio;
    //private GameObject soundSource;
    //private Vector2 soundSourceLocation;
    //private bool soundSourceLoop;

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
        //if (lowerTheMusicVolume)
        //    MusicFade();

        if (lowerTheSelectedMusicVolume)
            MusicFade();

        //if (increaseTheMusicVolume)
        //    MusicIncrease();

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

    //public void SelectAudio(AudioSource audio)
    //{
    //    _audio = audio;
    //}

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

    //public void IntenseStart(float volume)
    //{
    //    if(!_audioController.intenseMusic.isPlaying)
    //    {
    //        increaseTheMusicVolume = false;
    //        lowerTheMusicVolume = false;
    //        _audioController.StopAllMusic();
    //        _audioController.intenseMusic.volume = volume;
    //        _audioController.intenseMusic.Play();
    //    }
    //}

    public void StopSelectedAudio()
    {
        if(_sfx != null)
             _sfx.Stop();

        if (_music != null)
            _music.Stop();
            
        //_audio.Stop();
        //_audioController.intenseMusic.Stop();
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
        //increaseTheSelectedSoundVolume = false;
        //lowerTheSelectedSoundVolume = true;    
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
        //increaseTheSelectedSoundVolume = false;
        //lowerTheSelectedSoundVolume = true;    
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

    //public void SoundSourceToInstantiate(GameObject go)
    //{
    //    soundSource = go;
    //}

    //public void SoundSourceLocation(Vector2 location)
    //{
    //    soundSourceLocation = location;
    //}


    //public void InstantiateSoundSource()
    //{
    //    Instantiate(soundSource, soundSourceLocation, Quaternion.identity);
    //}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject == player)
    //    {
    //        switch (audioOptions)
    //        {
    //            case AudioOptions.Start:

    //                upIsTriggered = false;
    //                downIsTriggered = false;
    //                if (!_audio.isPlaying)
    //                {

    //                    increaseTheSelectedSoundVolume = false;
    //                    lowerTheSelectedSoundVolume = false;
    //                    _audioController.StopAllMusic();
    //                    _audio.volume = desiredVolume;
    //                    _audio.Play();
    //                    if (destroyOnTrigger)
    //                        Destroy(gameObject);
    //                }
    //                break;

    //            case AudioOptions.Stop:

    //                _audio.Stop();
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case AudioOptions.FadeOut:

    //                upIsTriggered = false;
    //                downIsTriggered = false;
    //                increaseTheSelectedSoundVolume = false;
    //                lowerTheSelectedSoundVolume = true;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case AudioOptions.FadeIn:
    //                upIsTriggered = false;
    //                downIsTriggered = false;
    //                lowerTheSelectedSoundVolume = false;
    //                increaseTheSelectedSoundVolume = true;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case AudioOptions.FadeOutAndIn:

    //                if (_audioToFadeIn.isPlaying)
    //                {
    //                    break;
    //                }

    //                canBeFaded = true;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case AudioOptions.PitchUp:
    //                downIsTriggered = false;
    //                upIsTriggered = true;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //            case AudioOptions.PitchDown:
    //                upIsTriggered = false;
    //                downIsTriggered = true;
    //                if (destroyOnTrigger)
    //                    Destroy(gameObject);
    //                break;

    //        }
    //    }
    //}
    //public enum AudioOptions
    //{
    //    Start,
    //    Stop,
    //    FadeOut,
    //    FadeIn,
    //    FadeOutAndIn,
    //    PitchUp,
    //    PitchDown
    //};
}
