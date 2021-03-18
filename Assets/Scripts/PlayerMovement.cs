using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private LayerMask whatIsGround;
    private Transform _groundCheck;
    private Rigidbody2D _rigidbody2D;
    private bool _isGrounded = false;
    private const float GroundedRadius = .3f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _groundCheck = transform.GetChild(0);
    }

    private void Update()
    {
        CheckIsGrounded();
        Movement();
    }
    private void CheckIsGrounded()
    {
        var wasGrounded = _isGrounded;
        _isGrounded = false;

        var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, whatIsGround);
        foreach (var t in colliders)
        {
            if (t.gameObject != gameObject)
            {
                if (!wasGrounded)
                    _isGrounded = true;
            }
        }
    }


    private void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.AddForce(Vector2.right);
        }
        
        if (Input.GetKeyDown(KeyCode.W) && _isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody2D.AddForce(Vector2.left);
        }
    }
}
