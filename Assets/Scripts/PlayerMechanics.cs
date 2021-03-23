using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public bool foundItem = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Pickable")) return;
        other.gameObject.SetActive(false);
        foundItem = true;
    }
}
