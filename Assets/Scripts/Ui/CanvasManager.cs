using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }
        public CanvasGroup noteInventory, upgradeInventory, rocketInventory;
        public GameObject uiButton;
        public GameObject notesByLocation;
        public GameObject floatingText;
        public GameObject hud;
        public GameObject noteScreen;
        public GameObject rocketButton;
        public Transform storedNotesScreen;
        public Transform upgradeGrid;
        public TextMeshProUGUI upgradeText;
        public Slider fuelSlider;
        private Transform _currentNoteScreen;
        private float _currentNoteScreenHeight;
        private string _currentLocation;
        private GameObject _interact;
        private CanvasGroup _currentInfoScreen;
        private SFX _audioController;
        private Image _noteImage;

        private void Awake()
        {
            Instance = this;
            _currentNoteScreenHeight = 0;
            rocketButton.SetActive(false);
        }

        private void Start()
        {
            _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
            gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            _currentNoteScreen = null;
            _currentInfoScreen = noteInventory;
            hud.transform.DORotate(new Vector3(0,0,90), .3f)
                .SetUpdate(true);
        }

        public void SetHudActive()
        {
            var rect = hud.GetComponent<RectTransform>();
            if (rect.anchoredPosition.y != 0)
            {
                rect.DOAnchorPos(new Vector2(30, 0), .3f)
                    .SetUpdate(true);
                rect.DORotate(Vector3.zero, .3f)
                    .SetUpdate(true);
                PauseGame();
                _audioController.PlayOpenInventory();
            }
            else
            {
                rect.DOAnchorPos(new Vector2(30, -500), .3f)
                    .SetUpdate(true);
                rect.DORotate(new Vector3(0,0,90), .3f)
                    .SetUpdate(true);
                ResumeGame();
                _audioController.PlayCloseInventory();
            }
        }
        public void SetFuel(int fuel)
        {
            fuelSlider.value = fuel;
        }
        public void AddNewUpgrade(Sprite sprite, string stats)
        {
            var upgrade = Instantiate(uiButton, upgradeGrid);
            upgrade.GetComponent<Image>().sprite = sprite;
            var btn = upgrade.GetComponent<Button>();
            btn.onClick.AddListener(() => { upgradeText.text = stats; });
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }
        public void ResumeGame()
        {
            if (hud.GetComponent<RectTransform>().anchoredPosition.y < -500)return;
            Time.timeScale = 1;
        }

        public void ShowText(string note, Sprite sprite)
        {
            var rect = hud.GetComponent<RectTransform>();
            if (rect.anchoredPosition.y != 0)
                SetHudActive();
            noteScreen.SetActive(true);
            noteScreen.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = note;
            if (!_noteImage)
                _noteImage = noteScreen.transform.GetChild(0).GetComponentInChildren<Image>();
            if (sprite)
            {
                _noteImage.gameObject.SetActive(true);
                _noteImage.sprite = sprite;
            }
            else
                _noteImage.gameObject.SetActive(false);
            PauseGame();
        }

        public void AddNewNote(Sprite sprite, Sprite picture, string note, string location)
        {
            if (!_currentNoteScreen || location != _currentLocation)
            {
                var current = Instantiate(notesByLocation, storedNotesScreen);
                _currentNoteScreen = current.GetComponentInChildren<HorizontalLayoutGroup>().transform;
                _currentLocation = location;
            
                // get height of spawned notesbylocation element
                var heightNewElement = current.GetComponent<RectTransform>().sizeDelta.y;
                // calculate new height of children on stored notes screen
                _currentNoteScreenHeight += heightNewElement;
                // get current height of stored note screen
                var heightStored = storedNotesScreen.GetComponent<RectTransform>().sizeDelta.y;
                // if children's combined height exceeds current height of note screen calculate new height to fit all children
                if (_currentNoteScreenHeight > heightStored)
                {
                    var widthStored = storedNotesScreen.GetComponent<RectTransform>().sizeDelta.x;
                    storedNotesScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(widthStored, _currentNoteScreenHeight);
                }
                current.GetComponentInChildren<TextMeshProUGUI>().text = _currentLocation;
            }
            var obj = Instantiate(uiButton, _currentNoteScreen);
            obj.GetComponent<Image>().sprite = sprite;
            var btn = obj.GetComponent<Button>();
            btn.onClick.AddListener(() => { ShowText(note, picture);});
        }

        public void OpenNoteInventory()
        {
            _audioController.PlayCloseInventory();
            _currentInfoScreen
                .DOFade(0, 0.3f)
                .SetUpdate(true);
            _currentInfoScreen.interactable = false;
            _currentInfoScreen.blocksRaycasts = false;
            noteInventory
                .DOFade(1, 0.3f)
                .SetUpdate(true);
            noteInventory.interactable = true;
            noteInventory.blocksRaycasts = true;
            _currentInfoScreen = noteInventory;
        }
        public void OpenUpgradeInventory()
        {
            _audioController.PlayCloseInventory();
            _currentInfoScreen
                .DOFade(0, 0.3f)
                .SetUpdate(true);
            _currentInfoScreen.interactable = false;
            _currentInfoScreen.blocksRaycasts = false;
            upgradeInventory
                .DOFade(1, 0.3f)
                .SetUpdate(true);
            upgradeInventory.interactable = true;
            upgradeInventory.blocksRaycasts = true;
            _currentInfoScreen = upgradeInventory;
        }
        public void OpenRocketInventory()
        {
            _audioController.PlayCloseInventory();
            if(!rocketButton.activeSelf)
                rocketButton.SetActive(true);
            _currentInfoScreen
                .DOFade(0, 0.3f)
                .SetUpdate(true);
            _currentInfoScreen.interactable = false;
            _currentInfoScreen.blocksRaycasts = false;
            rocketInventory
                .DOFade(1, 0.3f)
                .SetUpdate(true);
            rocketInventory.interactable = true;
            rocketInventory.blocksRaycasts = true;
            _currentInfoScreen = rocketInventory;
        }

        public void HideInteract()
        {
            _interact.SetActive(false);
        }
        public void ShowInteract(Transform goT)
        {
            if (!_interact || !_interact.activeSelf)
            {
                _interact = Instantiate(floatingText, goT.position + Vector3.up, quaternion.identity, goT);
                return;
            }
            _interact.SetActive(true);
        }

        public void ShowRocketButton()
        {
            rocketButton.SetActive(true);
        }
    }
}
