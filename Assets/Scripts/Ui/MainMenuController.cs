using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        public RectTransform credits;
        public CanvasGroup fader, buttons;
        public AudioSource titleScreenMusic;


        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            fader.alpha = 1;
            fader.DOFade(0, 1).SetUpdate(true);
            MenuMusic();
        }

        private void MenuMusic()
        {
            titleScreenMusic.Play();
            var volume = titleScreenMusic.volume;
            titleScreenMusic.volume = 0;
            titleScreenMusic.DOFade(volume, 2).SetUpdate(true);
        }

        public void LoadGame(bool loadingLastSave)
        {
            if (loadingLastSave && SaveAndLoad.LoadStatus().Scene == null)
            {
                Debug.Log("No valid save");
                return;
            }
            
            StartCoroutine(LoadYourAsyncScene(loadingLastSave));
        }

        private IEnumerator LoadYourAsyncScene(bool loadingLastSave)
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
            
            var asyncLoad = loadingLastSave ? SaveAndLoad.StartLoadingSave() : SceneManager.LoadSceneAsync(1);
            
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
