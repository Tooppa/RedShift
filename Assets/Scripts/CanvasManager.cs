using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    public GameObject screenImage;
    public GameObject noteImage;
    public GameObject tooltip;
    public GameObject hud;
    public GameObject noteScreen;
    public Transform storedNotesScreen;
    public Transform pickableScreen;
    public Transform upgradeScreen;
    public Slider healthSlider;
    public Slider fuelSlider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
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
    public void AddNewUpgrade(Sprite sprite, string stats)
    {
        var image = Instantiate(screenImage, upgradeScreen);
        image.GetComponent<Image>().sprite = sprite;
        var spawnedTooltip = Instantiate(tooltip, image.transform);
        var pos = spawnedTooltip.transform.position;
        spawnedTooltip.transform.position = new Vector3(pos.x + 1, pos.y - .5f , pos.z);
        spawnedTooltip.GetComponent<TooltipScript>().ShowTooltip(stats);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        if (hud.activeSelf)return;
        Time.timeScale = 1;
    }

    public void ShowText(string note)
    {
        noteScreen.SetActive(true);
        noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
        PauseGame();
    }

    public void AddNewNote(GameObject go)
    {
        var obj = Instantiate(noteImage, storedNotesScreen);
        obj.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite;
        var btn = obj.GetComponent<Button>();
        btn.onClick.AddListener(() => { ShowText(go.GetComponent<Pickables>().getNote());});
    }
}
