using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager Instance { get; private set; }
        public CanvasGroup noteInventory, upgradeInventory, rocketInventory, 
            logScreen, bluePrintScreen, pauseMenu, fader, buttons;
        public RectTransform options;
        public GameObject uiButton, notesByLocation, floatingText, hud, 
            noteScreen, rocketButton;
        public Transform storedNotesScreen, upgradeGrid;
        public TextMeshProUGUI upgradeText;
        public Slider fuelSlider;
        public Image fuelIcon;
        public TMP_InputField sliderText;
        public Slider slider;
        private Transform _currentNoteScreen;
        private float _currentNoteScreenHeight;
        private string _currentLocation;
        private GameObject _interact;
        private CanvasGroup _currentInfoScreen;
        private SFX _audioController;
        private Image _noteImage;
        private float _volume;

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
            hud.transform
                .DORotate(new Vector3(0, 0, 90), .3f)
                .SetUpdate(true);
            StartCoroutine(FuelIconBlinker());
            ResumeGame();
        }

        private IEnumerator FuelIconBlinker()
        {
            while (true)
            {
                if (fuelSlider.value == 0)
                {
                    fuelIcon.DOBlendableColor(Color.white, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(.2f);
                    fuelIcon.DOBlendableColor(Color.red, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(.5f);
                }
                else if (fuelSlider.value > 0 && 3 > fuelSlider.value)
                {
                    fuelIcon.DOBlendableColor(Color.white, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(.2f);
                    fuelIcon.DOBlendableColor(Color.yellow, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(1);
                }
                else
                {
                    fuelIcon.DOBlendableColor(Color.white, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(.2f);
                    fuelIcon.DOBlendableColor(Color.green, .2f).SetUpdate(true);
                    yield return new WaitForSecondsRealtime(5f);
                }
            }
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
        public void SetPauseMenuActive()
        {
            if (pauseMenu.alpha == 0)
            {
                pauseMenu.DOFade(1, .3f).SetUpdate(true);
                pauseMenu.interactable = true;
                pauseMenu.blocksRaycasts = true;
                PauseGame();
                _audioController.PlayOpenInventory();
            }
            else
            {
                pauseMenu.DOFade(0, .3f).SetUpdate(true);
                pauseMenu.interactable = false;
                pauseMenu.blocksRaycasts = false;
                ResumeGame();
                _audioController.PlayCloseInventory();
            }
        }
        public void OpenMainMenu()
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
            var asyncLoad = SceneManager.LoadSceneAsync(0);
            asyncLoad.allowSceneActivation = sceneActivation;
            while (!asyncLoad.isDone)
            {
                asyncLoad.allowSceneActivation = sceneActivation;
                yield return new WaitForEndOfFrame();
            }
            ResumeGame();
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
            if (_currentInfoScreen == noteInventory) return;
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
            if (_currentInfoScreen == upgradeInventory) return;
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
            if (_currentInfoScreen == rocketInventory) return;
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

        public void OpenBluePrintScreen()
        {
            if (bluePrintScreen.interactable) return;
            _audioController.PlayCloseInventory();
            logScreen
                .DOFade(0, 0.3f)
                .SetUpdate(true);
            logScreen.interactable = false;
            logScreen.blocksRaycasts = false;
            bluePrintScreen
                .DOFade(1, 0.3f)
                .SetUpdate(true);
            bluePrintScreen.interactable = true;
            bluePrintScreen.blocksRaycasts = true;
        }
        public void OpenLogScreen()
        {
            if (logScreen.interactable) return;
            _audioController.PlayCloseInventory();
            bluePrintScreen
                .DOFade(0, 0.3f)
                .SetUpdate(true);
            bluePrintScreen.interactable = false;
            bluePrintScreen.blocksRaycasts = false;
            logScreen
                .DOFade(1, 0.3f)
                .SetUpdate(true);
            logScreen.interactable = true;
            logScreen.blocksRaycasts = true;
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
