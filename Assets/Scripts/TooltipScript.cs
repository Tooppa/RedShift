using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    private RectTransform _canvas;
    private TextMeshProUGUI _tooltipText;
    private RectTransform _backround;

    private void Start()
    {
        _backround = transform.Find("Background").GetComponent<RectTransform>();
        _tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void ShowTooltip(string tooltipString)
    {
        _tooltipText.SetText(tooltipString);
        _tooltipText.ForceMeshUpdate();
        var backGroundSize = _tooltipText.GetRenderedValues(false);
        _backround.sizeDelta = backGroundSize;
    }
}
