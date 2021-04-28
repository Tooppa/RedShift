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
    private RectTransform _options;
    private float _volume;
    private ResolutionManager _res;

    private void Start()
    {
        _res = ResolutionManager.Instance;
        _volume = AudioVolume.Instance.GetVolume();
        _options = this.GetComponent<RectTransform>();
    }

    public void ResolutionUp()
    {
        var resSpot = _res.GetResSpot();
        if (resSpot < _res.GetResLength() - 1)
            _res.SetResSpot(resSpot + 1);
        ChangeResText();
    }

    public void ResolutionDown()
    {
        var resSpot = _res.GetResSpot();
        if (resSpot > 0)
            _res.SetResSpot(resSpot - 1);
        ChangeResText();
    }

    private void ChangeResText()
    {
        var currentResolution = _res.GetResolution();
        resolutionText.text = ((int) currentResolution.x + ", " + (int) currentResolution.y);
    }

    public void ApplySettings()
    {
        var currentResolution = _res.GetResolution();
        _res.SetResolution(currentResolution, _res.GetFullScreen());
        AudioVolume.Instance.SetVolume(_volume);
    }

    public void ToggleFullscreen()
    {
        _res.ToggleFullScreen();
        toggle.isOn = _res.GetFullScreen();
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
        toggle.isOn = _res.GetFullScreen();
        ChangeResText();
        if (AudioVolume.Instance is { })
        {
            var volume = AudioVolume.Instance.GetVolume();
            VolumeSlider(volume);
            AdjustSlider(volume.ToString());
        }

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

