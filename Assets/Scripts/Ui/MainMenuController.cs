using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        public RectTransform options, credits;
        public CanvasGroup fader, buttons;
        public TMP_Text resolutionText;
        public Vector2[] resolutions;
        public int startResolution;

        private bool _fullScreen;
        private Vector2 _currentResolution;
        private int _resSpot;

        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            _fullScreen = Screen.fullScreen;
            _resSpot = startResolution;
            _currentResolution = resolutions[_resSpot];
            ChangeResText();
            Screen.SetResolution((int)_currentResolution.x, (int)_currentResolution.y, _fullScreen);
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
                .OnComplete(() =>
                {
                    sceneActivation = true;
                });
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
        }

        private IEnumerator QuitApplication()
        {
            yield return new WaitForEndOfFrame();
            fader
                .DOFade(1, .5f)
                .OnComplete(Application.Quit);
        }
        
        public void ToggleFullscreen()
        {
            _fullScreen = !Screen.fullScreen;
        }

        public void OpenOptions()
        {
            buttons
                .DOFade(0, .1f)
                .SetUpdate(true);
            options
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
        }
        public void OpenCredits()
        {
            buttons
                .DOFade(0, .1f)
                .SetUpdate(true);
            credits
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
        }
        public void CloseOptions()
        {
            buttons
                .DOFade(1, .1f)
                .SetUpdate(true);
            options
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
        public void CloseCredits()
        {
            buttons
                .DOFade(1, .1f)
                .SetUpdate(true);
            credits
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
    }
}
