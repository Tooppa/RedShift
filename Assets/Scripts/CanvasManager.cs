using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]private GameObject screenImage;
    private GameObject _hud;
    private GameObject _noteScreen;
    private GameObject _pickableScreen;

    private void Awake()
    {
        _hud = transform.GetChild(0).gameObject;
        _pickableScreen = _hud.transform.GetChild(0).gameObject;
        _noteScreen =  transform.GetChild(1).gameObject;
        _hud.SetActive(false);
    }

    public void AddNewImage(Sprite sprite)
    {
        var obj = Instantiate(screenImage, _pickableScreen.transform);
        obj.GetComponent<Image>().sprite = sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            _hud.SetActive(!_hud.activeSelf);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ShowText(string note)
    {
        _noteScreen.SetActive(true);
        _noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        PauseGame();
    }
}
