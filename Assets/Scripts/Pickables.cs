using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Pickables : MonoBehaviour
{
    [SerializeField]
    private PickableObjects data;
    private SpriteRenderer _spriteRenderer;
    public bool IsNote { private set; get; }
    public bool HasFuel { private set; get; }
    public bool RocketBoots { private set; get; }
    public bool Gun { private set; get; }
    public bool Flashlight { private set; get; }
    [HideInInspector]public int fuel;

    private void Awake()
    {
        if(TryGetComponent(out _spriteRenderer))
            _spriteRenderer.sprite = data.sprite;
        IsNote = data.note.Length > 0;
        HasFuel = data.fuel > 0;
        RocketBoots = data.rocketBoots;
        Gun = data.gun;
        Flashlight = data.flashlight;
        fuel = data.fuel;
        var light2D = transform.GetComponentInChildren<Light2D>();
        if (light2D)
            light2D.enabled = Flashlight;

    }

    public string GetNote()
    {
        return data.note;
    }

    public string GetStats()
    {
        return data.statsForUpgrades;
    }
}
