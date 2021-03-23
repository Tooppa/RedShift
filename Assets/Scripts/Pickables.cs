using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickables : MonoBehaviour
{
    public PickableObjects data;
    private new string name;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        name = data.name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
    }
}
