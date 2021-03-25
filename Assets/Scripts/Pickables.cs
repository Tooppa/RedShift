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
    public bool IsNote { private set; get; }
    private GameObject _interact;

    private void Awake()
    {
        name = data.name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
        IsNote = data.note.Length > 0;
        if (!IsNote) return;
        _note = data.note;
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
        return _note;
    }
}
