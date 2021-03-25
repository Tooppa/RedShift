using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject pickableScreen;
    [SerializeField] private GameObject noteScreen;
    [SerializeField] private GameObject screenImage;

    public void AddNewImage(Sprite sprite)
    {
        var obj = Instantiate(screenImage, pickableScreen.transform);
        obj.GetComponent<Image>().sprite = sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            pickableScreen.SetActive(!pickableScreen.activeSelf);
    }

    public void ShowText(string note)
    {
        noteScreen.SetActive(true);
        noteScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = note;
    }
}
