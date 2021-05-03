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
            _interact = Instantiate(floatingText, transform.position + Vector3.up, quaternion.identity);
            return;
        }

        _interact.SetActive(true);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerMechanics>().ChangeInteract().started += _ => Play();
        _onTrigger = true;
        ShowInteract();
    }

    private void Play()
    {
        if(!_onTrigger) return;
        _playable.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _onTrigger = false;
        HideInteract();
    }
}
