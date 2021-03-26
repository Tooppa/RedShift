using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pickables : MonoBehaviour
{
    public PickableObjects data;
    private SpriteRenderer _spriteRenderer;
    public bool IsNote { private set; get; }
    public bool HasFuel { private set; get; }
    private GameObject _interact;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
        IsNote = data.note.Length > 0;
        HasFuel = data.fuel > 0;
    }

    public void ShowInteract()
    {
        if (!_interact)
        {
            _interact = Instantiate(data.floatingText, transform.position + Vector3.up, quaternion.identity, transform);
            return;
        }
        _interact.SetActive(true);
    }

    public void HideInteract()
    {
        _interact.SetActive(false);
    }

    public string getNote()
    {
        return data.note;
    }
}
