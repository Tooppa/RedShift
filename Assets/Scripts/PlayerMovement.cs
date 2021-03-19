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
    private Animator _animator;
    private const float GroundedRadius = .05f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _groundCheck = transform.GetChild(0);
    }

    private void Update()
    {
        CheckIsGrounded();
        Movement();
    }
    private void CheckIsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, GroundedRadius, whatIsGround);
    }


    private void Movement()
    {
        var inputDirection = Input.GetAxisRaw("Horizontal");

        if(inputDirection != 0)
            transform.localScale = new Vector3(inputDirection, 1, 1);

        if (Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
        {
            _animator.SetBool("Walking", false);
            _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);
        }
        else _animator.SetBool("Walking", true);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        
    }
}