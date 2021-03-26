using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    public Dictionary<string, GameObject> FoundItems;

    private void Awake()
    {
        FoundItems = new Dictionary<string, GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickable")) return;

        var go = other.gameObject;
        var pickables = go.GetComponent<Pickables>();

        if (pickables.IsNote)
        {
            pickables.ShowInteract();
            return;
        }
        go.SetActive(false);
        FoundItems.Add(other.name, go);
        CanvasManager.Instance.AddNewImage(go.GetComponent<SpriteRenderer>().sprite);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var pickables = other.gameObject.GetComponent<Pickables>();
        if (!pickables.IsNote || !Input.GetKey(KeyCode.E)) return;
        CanvasManager.Instance.ShowText(pickables.getNote());
        other.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pickables = other.gameObject.GetComponent<Pickables>();
        if (pickables.IsNote)
            pickables.HideInteract();
    }
}
