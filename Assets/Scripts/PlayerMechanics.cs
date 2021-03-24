using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public bool foundItem = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickable")) return;
        other.gameObject.SetActive(false);
        foundItem = true;
    }
}
