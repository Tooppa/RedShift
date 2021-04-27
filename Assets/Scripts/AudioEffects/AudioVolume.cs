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
        if (Instance != null && Instance != this)
            Destroy(this); 
        else Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SetVolume(100);
    }

    public void SetVolume(float volume)
    {
        _volume = volume;
        AudioListener.volume = _volume/100;
    }

    public float GetVolume()
    {
        return _volume;
    }
}
