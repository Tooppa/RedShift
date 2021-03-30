using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    public void ShowTooltip(string tooltipString)
    {
        var tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        var backGround = transform.Find("Background").GetComponent<RectTransform>();
        
        tooltipText.text = tooltipString;
        tooltipText.ForceMeshUpdate();
        var padding = tooltipString.Length > 0 ? new Vector2(2, 2): Vector2.zero;
        backGround.sizeDelta = tooltipText.GetPreferredValues() + padding;
    }
}
