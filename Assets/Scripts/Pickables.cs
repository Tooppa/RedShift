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
    private GameObject _interact;
    [HideInInspector]public int fuel;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = data.sprite;
        IsNote = data.note.Length > 0;
        HasFuel = data.fuel > 0;
        RocketBoots = data.rocketBoots;
        Gun = data.gun;
        Flashlight = data.flashlight;
        fuel = data.fuel;
        transform.GetComponentInChildren<Light2D>().enabled = Flashlight;
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

    public string GetStats()
    {
        return data.statsForUpgrades;
    }
}
