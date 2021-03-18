using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float maxSpeed;
    private Transform _groundCheck;
    private Rigidbody2D _rigidbody2D;
    private bool _isGrounded = false;
    private const float GroundedRadius = .05f;

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
        _isGrounded = false;
        var colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, whatIsGround);
        _isGrounded = colliders.Length > 0;
    }


    private void Movement()
    {
        var inputDirection = Input.GetAxisRaw("Horizontal");
        
        if(Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
            _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        
    }
}