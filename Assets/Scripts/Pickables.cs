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
    private string _note;
    private bool _isNote;
    

    private void Awake()
    {
        name = data.name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
        _isNote = data.note.Length > 0;
        if (!_isNote) return;
        _note = data.note;
    }

    public void ShowInteract()
    {
        Instantiate(data.floatingText, transform.position + Vector3.up, quaternion.identity, transform);
    }

    public void HideInteract()
    {
        throw new NotImplementedException();
    }
}
