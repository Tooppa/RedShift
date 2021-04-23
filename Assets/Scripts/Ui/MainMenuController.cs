using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        public RectTransform options, credits;
        public CanvasGroup fader;
        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
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
            
        }
        public void OpenOptions()
        {
            options
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
        }
        public void OpenCredits()
        {
            credits
                .DOAnchorPos(Vector2.zero, .3f)
                .SetUpdate(true);
        }
        public void CloseOptions()
        {
            options
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
        public void CloseCredits()
        {
            credits
                .DOAnchorPos(new Vector2(0, -500), .3f)
                .SetUpdate(true);
        }
    }
}
