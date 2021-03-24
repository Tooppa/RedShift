using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private Dictionary<string, GameObject> _foundItems;
    private void Awake()
    {
        _foundItems = new Dictionary<string, GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickable")) return;
        var go= other.gameObject;
        go.SetActive(false);
        _foundItems.Add(other.name, go);
    }
}
