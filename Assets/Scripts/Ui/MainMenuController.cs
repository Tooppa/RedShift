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

        private float _volume;
        private ResolutionManager res;

        private void Start()
        {
            res = ResolutionManager.Instance;
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            fader.alpha = 1;
            fader.DOFade(0, 1).SetUpdate(true);
            _volume = AudioVolume.Instance.GetVolume();
            MenuMusic();
        }

        private void MenuMusic()
        {
            _volume = AudioVolume.Instance.GetVolume();
            AudioVolume.Instance.SetVolume(_volume);
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
            var resSpot = res.GetResSpot();
            if (resSpot < res.GetResLength() - 1)
                res.SetResSpot(resSpot + 1);
            ChangeResText();
        }
        
        public void ResolutionDown()
        {
            var resSpot = res.GetResSpot();
            if (resSpot > 0) 
                res.SetResSpot(resSpot - 1);
            ChangeResText();
        }

        private void ChangeResText()
        {
            var currentResolution = res.GetResolution();
            resolutionText.text = ((int) currentResolution.x + ", " + (int) currentResolution.y);
        }

        public void ApplySettings()
        {
            var currentResolution = res.GetResolution();
            res.SetResolution(currentResolution, res.GetFullScreen());
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
            res.ToggleFullScreen();
            toggle.isOn = res.GetFullScreen();
        }

        public void OpenOptions()
        {
            toggle.isOn = res.GetFullScreen();
            ChangeResText();
            var volume = AudioVolume.Instance.GetVolume();
            VolumeSlider(volume);
            AdjustSlider(volume.ToString());
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
