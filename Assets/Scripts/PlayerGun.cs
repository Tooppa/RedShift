using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    private GameObject _player;
    private GameObject _gun;
    private GameObject _bullet;
    private GameObject _flashlight;

    private Vector2 _shootDirection;

    // Start is called before the first frame update
    void Start()
    {
        _shootDirection = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
