using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioTriggerController : MonoBehaviour
{
    private SFX _audioController;
    private MusicTriggerControl musicTriggerController;
    private GameObject player;
    public GameObject destroyableSoundSource;

    public AudioOptions audioOptions = new AudioOptions();

    public AudioSource _music;
    public AudioSource _sfx;
    public AudioSource _musicToFadeOut;
    public AudioSource _musicToFadeIn;
    public AudioSource _sfxToFadeOut;
    public AudioSource _sfxToFadeIn;

    public AudioMixerSnapshot[] audioSnapshot;
    public AudioMixer mixer;
    public float[] weights;

    public bool loop = false;

    public bool destroyOnTrigger = false;

    public float musicFadeTime = 4;
    public float sfxFadeTime = 4;
    public float volume;
    public float desiredIncreasedMusicVolume = 0;
    public float desiredIncreasedSFXVolume = 0;
    public float desiredFadedMusicVolume = 0;
    public float desiredFadedSFXVolume = 0;
    public float desiredPitch = 1;

    // Start is called before the first frame update
    void Start()
    {
        musicTriggerController = GameObject.Find("AudioEventSystem").GetComponent<MusicTriggerControl>();
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            if (_sfx != null)
            {
                musicTriggerController.SFX(_sfx);
            }
            else if (_music != null)
            {
                musicTriggerController.Music(_music);
            }
                
            switch (audioOptions)
            {
                case AudioOptions.MusicStart:
                    musicTriggerController.Loop(loop);
                    musicTriggerController.Volume(volume);
                    musicTriggerController.PlaySelectedMusic();
                    mixer.TransitionToSnapshots(audioSnapshot, weights, 3);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    
                    break;

                case AudioOptions.AudioStop:
                    musicTriggerController.StopSelectedAudio();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.SFXStart:
                    musicTriggerController.SFX(_sfx);
                    musicTriggerController.Volume(volume);
                    musicTriggerController.Loop(loop);
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.PlaySelectedSFX();
                    if (destroyOnTrigger)
                        Destroy(gameObject);

                    break;

                //case AudioOptions.SFXStop:
                //    musicTriggerController.SFX(_sfx);
                //    musicTriggerController.StopSelectedAudio();
                //    if (destroyOnTrigger)
                //        Destroy(gameObject);
                //    break;

                case AudioOptions.StopAllAudio:
                    _audioController.StopAllMusic();
                    _audioController.StopAllAudio();
                    break;

                case AudioOptions.FadeOutMusic:
                    musicTriggerController.MusicFadeTime(musicFadeTime);
                    musicTriggerController.DesiredFadedMusicVolume(desiredFadedMusicVolume);
                    musicTriggerController.MusicToFadeOut(_musicToFadeOut);
                    musicTriggerController.FadeOutSelectedMusic(desiredFadedMusicVolume);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeInMusic:
                    musicTriggerController.MusicFadeTime(musicFadeTime);
                    musicTriggerController.MusicToFadeIn(_musicToFadeIn);
                    musicTriggerController.FadeInSelectedMusic(desiredIncreasedMusicVolume);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOutSFX:
                    musicTriggerController.SFX(_sfx);
                    musicTriggerController.SFXFadeTime(sfxFadeTime);
                    musicTriggerController.DesiredFadedSFXVolume(desiredFadedSFXVolume);
                    musicTriggerController.SFXToFadeOut(_sfxToFadeOut);
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.FadeOutSelectedSFX(desiredFadedSFXVolume);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeInSFX:
                    musicTriggerController.SFXFadeTime(sfxFadeTime);
                    musicTriggerController.SFXToFadeIn(_sfxToFadeIn);
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.FadeInSelectedSFX(desiredIncreasedSFXVolume, desiredPitch);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOutAndInMusic:
                    musicTriggerController.MusicFadeTime(musicFadeTime);
                    musicTriggerController.DesiredIncreasedMusicVolume(desiredIncreasedMusicVolume);
                    musicTriggerController.DesiredFadedMusicVolume(desiredFadedMusicVolume);
                    musicTriggerController.MusicToFadeIn(_musicToFadeIn);
                    musicTriggerController.MusicToFadeOut(_musicToFadeOut);
                    musicTriggerController.FadeOutAndInMusic();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOutAndInSFX:
                    musicTriggerController.SFXFadeTime(sfxFadeTime);
                    musicTriggerController.DesiredIncreasedSFXVolume(desiredIncreasedSFXVolume);
                    musicTriggerController.DesiredFadedSFXVolume(desiredFadedSFXVolume);
                    musicTriggerController.SFXToFadeIn(_sfxToFadeIn);
                    musicTriggerController.SFXToFadeOut(_sfxToFadeOut);
                    musicTriggerController.Pitch(desiredPitch);
                    musicTriggerController.FadeOutAndInSFX();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchUpSFX:
                    musicTriggerController.SFXFadeTime(sfxFadeTime);
                    musicTriggerController.SFXPitchUp(desiredPitch);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchDownSFX:
                    musicTriggerController.SFXFadeTime(sfxFadeTime);
                    musicTriggerController.SFXPitchDown(desiredPitch);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PlaySFX:
                    musicTriggerController.Loop(loop);
                    musicTriggerController.Volume(volume);
                    musicTriggerController.Pitch(desiredPitch);
                    musicTriggerController.PlaySFX();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.DestroySoundSource:
                    Destroy(destroyableSoundSource);
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;
            }
        }
    }
    public enum AudioOptions
    {
        MusicStart,
        AudioStop,
        SFXStart,
        StopAllAudio,
        FadeOutMusic,
        FadeInMusic,
        FadeOutSFX,
        FadeInSFX,
        FadeOutAndInMusic,
        FadeOutAndInSFX,
        PitchUpSFX,
        PitchDownSFX,
        PlaySFX,
        DestroySoundSource
    };
}

