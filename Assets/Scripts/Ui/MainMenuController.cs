using DG.Tweening;
using UnityEngine;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        public RectTransform options, credits;
        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
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
