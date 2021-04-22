using UnityEngine;

namespace Ui
{
    public class MainMenuController : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
