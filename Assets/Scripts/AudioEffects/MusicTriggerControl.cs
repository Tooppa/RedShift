using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerControl : MonoBehaviour
{
    private SFX _audioController;
    //public SoundSourceScriptable soundSourceData;

    private AudioSource _audio;
    private AudioSource _audioToFadeOut;
    private AudioSource _audioToFadeIn;

    //private AudioSource instantiableAudio;
    //private GameObject soundSource;
    //private Vector2 soundSourceLocation;
    //private bool soundSourceLoop;

    private bool canBeFaded = false;
    private bool lowerTheSelectedSoundVolume = false;
    private bool increaseTheSelectedSoundVolume = false;
    private bool upIsTriggered = false;
    private bool downIsTriggered = false;
    private bool loop = false;

    private float fadeTime = 4;
    private float desiredVolume = 0.5f;
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
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
    }

    private void FixedUpdate()
    {
        //if (lowerTheMusicVolume)
        //    MusicFade();

        if (lowerTheSelectedSoundVolume)
            AudioFade();

        //if (increaseTheMusicVolume)
        //    MusicIncrease();

        if (increaseTheSelectedSoundVolume)
            AudioIncrease();

        if (downIsTriggered)
            DownPitcher();

        if (upIsTriggered)
            UpPitcher();

        if (canBeFaded)
            FadeOutAndInControl();

    }

    private void AudioIncrease()
    {
        lowerTheSelectedSoundVolume = false;
        if (_audioToFadeIn.volume >= desiredVolume)
        {
            increaseTheSelectedSoundVolume = false;
            _audioToFadeIn.volume = desiredVolume;
            return;
        }

        _audioToFadeIn.volume += Time.deltaTime * 1 / fadeTime;

        if (_audioToFadeIn.volume >= desiredVolume)
        {
            _audioToFadeIn.volume = desiredVolume;
            increaseTheSelectedSoundVolume = false;
        }
    }

    private void AudioFade()
    {
        increaseTheSelectedSoundVolume = false;
        if (_audioToFadeOut.volume == 0)
        {
            lowerTheSelectedSoundVolume = false;
            return;
        }

        _audioToFadeOut.volume -= Time.deltaTime * 1 / fadeTime;

        if (_audioToFadeOut.volume == 0)
        {
            //_audio.Pause();
            lowerTheSelectedSoundVolume = false;
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

    public void SelectAudio(AudioSource audio)
    {
        _audio = audio;
    }

    public void PlaySFX(AudioSource audio)
    {

        audio.Play();
        if (loop == true)
            audio.loop = true;
    }

    public void Loop(bool looped)
    {
        loop = looped;
    }

    public void StartSelectedAudio(float volume)
    {
        if(!_audioController.calmAmbience.isPlaying)
        {
            increaseTheSelectedSoundVolume = false;
            lowerTheSelectedSoundVolume = false;
            _audioController.StopAllMusic();
            _audio.volume = volume;
            _audio.Play();
        }
    }

    public void AudioToFadeOut(AudioSource fadeOutAudio)
    {
        _audioToFadeOut = fadeOutAudio;
    }

    public void AudioToFadeIn(AudioSource fadeInAudio)
    {
        _audioToFadeIn = fadeInAudio;
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

    public void StopSelectedMusic()
    {
        _audio.Stop();
        //_audioController.intenseMusic.Stop();
    }

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

    public void FadeOutSelectedAudio()
    {
        increaseTheSelectedSoundVolume = false;
        lowerTheSelectedSoundVolume = true;    
    }

    public void FadeInSelectedAudio(float volume)
    {
        desiredVolume = volume;
        lowerTheSelectedSoundVolume = false;
        increaseTheSelectedSoundVolume = true;
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

    public void FadeOutAndInAudio()
    {

        if (_audioToFadeIn.isPlaying)
        {
            return;
        }
        canBeFaded = true;
    }

    private void FadeOutAndInControl()
    {
        lowerTheSelectedSoundVolume = true;
        if(_audioToFadeOut.volume == 0 && canBeFaded)
        {
            lowerTheSelectedSoundVolume = false;
            _audioToFadeIn.volume = 0;
            _audioToFadeOut.Stop();
            _audioToFadeIn.Play();

            increaseTheSelectedSoundVolume = true;
            canBeFaded = false;
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

    public void DestroyGameObject(GameObject go)
    {
        Destroy(go);
    }
}
