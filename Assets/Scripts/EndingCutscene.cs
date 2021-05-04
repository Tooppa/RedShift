using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class EndingCutscene : MonoBehaviour
{
    public GameObject floatingText;
    private GameObject _interact;
    private PlayerControls _playerControls;
    private PlayableDirector _playable;
    private bool _onTrigger;
    private PlayerMechanics _player;

    private void Start()
    {
        _playable = GetComponentInParent<PlayableDirector>();
    }

    private void HideInteract()
    {
        _interact.SetActive(false);
    }

    private void ShowInteract()
    {
        if (!_interact || !_interact.activeSelf)
        {
            var transform1 = transform;
            _interact = Instantiate(floatingText, transform1.position + Vector3.up, quaternion.identity, transform1);
            return;
        }

        _interact.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _player = other.GetComponent<PlayerMechanics>();
        _player.ChangeInteract().started += _ => Play();
        _onTrigger = true;
        ShowInteract();
    }

    private void Play()
    {
        if(!_onTrigger || _player.GetFuel() < 1) return;
        _player.DisablePlayerLightsForSeconds(20);
        _playable.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _onTrigger = false;
        HideInteract();
    }
}
