using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent @event;
    public string collideWith = "";
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(collideWith))
            @event.Invoke();
    }
}
