using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        public RectTransform options, credits;
        public CanvasGroup fader, buttons;
        public TMP_Text resolutionText;
        public TMP_InputField sliderText;
        public Slider slider;
        public Toggle toggle;
        public AudioSource titleScreenMusic;
        public Vector2[] resolutions;
        public int startResolution;

        private bool _fullScreen;
        private Vector2 _currentResolution;
        private int _resSpot;
        private float _volume;

        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            _fullScreen = Screen.fullScreen;
            _resSpot = startResolution;
            _currentResolution = resolutions[_resSpot];
            ChangeResText();
            Screen.SetResolution((int)_currentResolution.x, (int)_currentResolution.y, _fullScreen);
            fader.alpha = 1;
            fader.DOFade(0, 1).SetUpdate(true);
            toggle.isOn = Screen.fullScreen;
            _volume = AudioVolume.Instance.GetVolume();
            MenuMusic();
        }

        private void MenuMusic()
        {
            AudioVolume.Instance.SetVolume(100);
            titleScreenMusic.volume = 0;
            titleScreenMusic.Play();
            titleScreenMusic.DOFade(1, 2).SetUpdate(true);
        }

        public void LoadGame()
        {
            StartCoroutine(LoadYourAsyncScene());
        }

        private IEnumerator LoadYourAsyncScene()
        {
            var sceneActivation = false;
            fader
                .DOFade(1, 1)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    sceneActivation = true;
                });
            titleScreenMusic.DOFade(0, 1).SetUpdate(true);
            var asyncLoad = SceneManager.LoadSceneAsync(1);
            asyncLoad.allowSceneActivation = sceneActivation;
            while (!asyncLoad.isDone)
            {
                asyncLoad.allowSceneActivation = sceneActivation;
                yield return new WaitForEndOfFrame();
            }
        }

        public void ExitGame()
        {
            StartCoroutine(QuitApplication());
        }

        public void ResolutionUp()
        {
            if (_resSpot < resolutions.Length - 1)
                _resSpot++;
            _currentResolution = resolutions[_resSpot];
            ChangeResText();
        }
        
        public void ResolutionDown()
        {
            if (_resSpot > 0) 
                _resSpot--;
            _currentResolution = resolutions[_resSpot];
            ChangeResText();
        }

        private void ChangeResText()
        {
            resolutionText.text = ((int) _currentResolution.x + ", " + (int) _currentResolution.y);
        }

        public void ApplySettings()
        {
            Screen.SetResolution((int)_currentResolution.x, (int)_currentResolution.y, _fullScreen);
            AudioVolume.Instance.SetVolume(_volume);
        }

        private IEnumerator QuitApplication()
        {
            yield return new WaitForEndOfFrame();
            fader
                .DOFade(1, .5f)
                .SetUpdate(true)
                .OnComplete(Application.Quit);
        }
        
        public void ToggleFullscreen()
        {
            _fullScreen = !Screen.fullScreen;
        }

        public void OpenOptions()
        {
            buttons
                .DOFade(0, .3f)
                .SetUpdate(true);
            options
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
        }
        public void OpenCredits()
        {
            buttons
                .DOFade(0, .3f)
                .SetUpdate(true);
            credits
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
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
        public void CloseOptions()
        {
            buttons
                .DOFade(1, .3f)
                .SetUpdate(true);
            options
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
        public void CloseCredits()
        {
            buttons
                .DOFade(1, .3f)
                .SetUpdate(true);
            credits
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
    }
}
