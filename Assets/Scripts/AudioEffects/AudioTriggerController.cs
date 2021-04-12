using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioTriggerController : MonoBehaviour
{
    //Laita t‰nne jotain muuttujia, joihin MusicTriggerControllerilla on p‰‰sy, ja ett‰ se laittaa t‰‰lt‰ parametrit omaan scriptiin
    //!!!
    //T‰m‰ scripti triggereille
    private SFX _audioController;
    private MusicTriggerControl musicTriggerController;
    private GameObject player;
    public AudioOptions audioOptions = new AudioOptions();

    public AudioSource _audio;
    public AudioSource _audioToFadeOut;
    public AudioSource _audioToFadeIn;

    //private bool canBeFaded = false;
    //private bool lowerTheSelectedSoundVolume = false;
    //private bool increaseTheSelectedSoundVolume = false;
    //private bool upIsTriggered = false;
    //private bool downIsTriggered = false;
    //private bool loop = false;

    public bool destroyOnTrigger = false;

    public float fadeTime = 4;
    public float desiredVolume = 0.5f;
    private float startingPitch = 1;
    public float desiredPitch;

    // Start is called before the first frame update
    void Start()
    {
        musicTriggerController = GameObject.Find("AudioEventSystem").GetComponent<MusicTriggerControl>();
        _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
        player = GameObject.Find("Player");

        //audioOptions = (AudioOptions)EditorGUILayout.EnumPopup(audioOptions);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            
            
            

            switch (audioOptions)
            {
                case AudioOptions.Start:
                    musicTriggerController._audio = _audio;
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    if (!musicTriggerController._audio.isPlaying)
                    {

                        musicTriggerController.increaseTheSelectedSoundVolume = false;
                        musicTriggerController.lowerTheSelectedSoundVolume = false;
                        _audioController.StopAllMusic();
                        musicTriggerController._audio.volume = desiredVolume;
                        musicTriggerController._audio.Play();
                        if (destroyOnTrigger)
                            Destroy(gameObject);
                    }
                    break;

                case AudioOptions.Stop:
                    musicTriggerController._audio = _audio;
                    musicTriggerController._audio.Stop();
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOut:
                    musicTriggerController._audioToFadeOut = _audioToFadeOut;
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.increaseTheSelectedSoundVolume = false;
                    musicTriggerController.lowerTheSelectedSoundVolume = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeIn:
                    musicTriggerController._audioToFadeIn = _audioToFadeIn;
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.lowerTheSelectedSoundVolume = false;
                    musicTriggerController.increaseTheSelectedSoundVolume = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.FadeOutAndIn:                   
                    musicTriggerController._audioToFadeIn = _audioToFadeIn;
                    if (musicTriggerController._audioToFadeIn.isPlaying)
                    {
                        break;
                    }
                    musicTriggerController._audioToFadeOut = _audioToFadeOut;
                    musicTriggerController.canBeFaded = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchUp:
                    musicTriggerController._audio = _audio;
                    musicTriggerController.desiredPitch = desiredPitch;
                    musicTriggerController.downIsTriggered = false;
                    musicTriggerController.upIsTriggered = true;
                    if (destroyOnTrigger)
                        Destroy(gameObject);
                    break;

                case AudioOptions.PitchDown:
                    musicTriggerController._audio = _audio;
                    musicTriggerController.desiredPitch = desiredPitch;
                    musicTriggerController.upIsTriggered = false;
                    musicTriggerController.downIsTriggered = true;
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

