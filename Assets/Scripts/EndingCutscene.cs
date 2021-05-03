using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EndingCutscene : MonoBehaviour
{
    public GameObject floatingText;
    private GameObject _interact;

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
        ShowInteract();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        HideInteract();
    }
}
