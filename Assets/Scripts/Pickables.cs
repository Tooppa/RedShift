using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pickables : MonoBehaviour
{
    public PickableObjects data;
    private new string name;
    private SpriteRenderer _spriteRenderer;
    public string note;
    private bool _isNote;
    private GameObject _interact;

    private void Awake()
    {
        name = data.name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
        _isNote = data.note.Length > 0;
        if (!_isNote) return;
        note = data.note;
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
}
