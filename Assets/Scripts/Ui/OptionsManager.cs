using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public TMP_Text resolutionText;
    public Toggle toggle;
    public CanvasGroup buttons;
    public TMP_InputField sliderText;
    public Slider slider;
    public Vector2[] resolutions;
    private bool _fullScreen;
    private int _resSpot;
    private RectTransform _options;
    private float _volume;

    private void Start()
    {
        _options = GetComponent<RectTransform>();
    }

    private void SetVolume(float volume)
    {
        _volume = volume;
        AudioListener.volume = _volume/100;
    }
    public void ResolutionUp()
    {
        if (_resSpot < resolutions.Length - 1)
            _resSpot++;
        ChangeResText();
    }

    public void ResolutionDown()
    {
        if (_resSpot > 0)
            _resSpot--;
        ChangeResText();
    }

    private void ChangeResText()
    {
        var currentResolution = resolutions[_resSpot];
        resolutionText.text = ((int) currentResolution.x + ", " + (int) currentResolution.y);
    }

    public void ApplySettings()
    {
        var currentResolution = resolutions[_resSpot];
        Screen.SetResolution((int)currentResolution.x, (int)currentResolution.y, _fullScreen);
        SetVolume(_volume);
        PlayerPrefs.SetInt("Res", _resSpot);
        PlayerPrefs.SetFloat("Vol", _volume);
        PlayerPrefs.SetInt("FullS", _fullScreen ? 1 : 0);
    }

    public void ToggleFullscreen()
    {
        _fullScreen = !_fullScreen;
    }
    
    public void VolumeSlider(float volume)
    {
        _volume = volume;
        sliderText.text = _volume.ToString(); 
    }

    public void AdjustSlider(string volume)
    {
        _volume = int.Parse(volume);
        slider.value = _volume;
    }

    public void OpenOptions()
    {
        _resSpot = PlayerPrefs.GetInt("Res");
        _volume = PlayerPrefs.GetFloat("Vol");
        toggle.isOn = PlayerPrefs.GetInt("FullS") == 1;
        ChangeResText();
        VolumeSlider(_volume);
        AdjustSlider(_volume.ToString());

        buttons
            .DOFade(0, .3f)
            .SetUpdate(true);
        _options
            .DOAnchorPos(Vector2.zero, .3f)
            .SetUpdate(true);
    }
    public void CloseOptions()
    {
        buttons
            .DOFade(1, .3f)
            .SetUpdate(true);
        _options
            .DOAnchorPos(new Vector2(0, -500), .3f)
            .SetUpdate(true);
    }
}

