using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    private TextMeshProUGUI _tooltipText;
    private RectTransform _backGround;

    private void Start()
    {
        _backGround = transform.GetChild(0).GetComponent<RectTransform>();
        _tooltipText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ShowTooltip(string tooltipString)
    {
        _tooltipText.text = tooltipString;
        _tooltipText.ForceMeshUpdate();
        var backGroundSize = _tooltipText.GetRenderedValues(false);
        _backGround.sizeDelta = backGroundSize;
    }
}
