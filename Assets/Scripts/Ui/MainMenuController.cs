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
        public AudioSource titleScreenMusic;

        private float _volume;

        private void Start()
        {
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
            titleScreenMusic.Play();
            titleScreenMusic.DOFade(.7f, 2).SetUpdate(true);
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

        private IEnumerator QuitApplication()
        {
            yield return new WaitForEndOfFrame();
            fader
                .DOFade(1, .5f)
                .SetUpdate(true)
                .OnComplete(Application.Quit);
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
