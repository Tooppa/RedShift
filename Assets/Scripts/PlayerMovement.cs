using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int speed;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _rigidbody2D.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
        }
    }
}
