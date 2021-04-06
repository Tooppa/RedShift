using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private float _waitTime;

    

    IEnumerator ButtonPress()
    {
        yield return new WaitForSeconds(_waitTime); // Wait for animation

    }
}
