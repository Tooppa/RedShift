using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public static AudioVolume Instance { get; private set; }
    private float _volume;

    private void Awake()
    {
        if(!AudioVolume.Instance)
            Instance = this;
    }

    private void Start()
    {
        AudioListener.volume = _volume;
    }

    public void SetVolume(float volume)
    {
        _volume = volume/100;
        AudioListener.volume = _volume;
    }

    public float GetVolume()
    {
        return _volume;
    }
}
