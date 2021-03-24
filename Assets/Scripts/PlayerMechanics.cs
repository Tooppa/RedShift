using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    [SerializeField] private CanvasManager canvas;
    public Dictionary<string, GameObject> FoundItems;
    private void Awake()
    {
        FoundItems = new Dictionary<string, GameObject>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickable")) return;
        var go= other.gameObject;
        go.SetActive(false);
        FoundItems.Add(other.name, go);
        canvas.AddNewImage(go.GetComponent<SpriteRenderer>().sprite);
    }
}
