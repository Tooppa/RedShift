using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    public GameObject screenImage;
    public GameObject hud;
    public GameObject noteScreen;
    public Transform pickableScreen;
    public Slider healthSlider;
    public Slider fuelSlider;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab)) return;
        
        hud.SetActive(!hud.activeSelf);
        if(hud.activeSelf)
            PauseGame();
        else
            ResumeGame();
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
    public void SetFuel(int fuel)
    {
        fuelSlider.value = fuel;
    }

    public void AddNewImage(Sprite sprite)
    {
        var obj = Instantiate(screenImage, pickableScreen);
        obj.GetComponent<Image>().sprite = sprite;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ShowText(string note)
    {
        noteScreen.SetActive(true);
        noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        PauseGame();
    }
}
