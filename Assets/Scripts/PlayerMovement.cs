using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float speed;

    [SerializeField] 
    private float maxSpeed;
    
    [SerializeField] 
    private float jumpSpeed;
    
    private bool isGrounded = true;
    
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var inputDirection = Input.GetAxisRaw("Horizontal");
        
        if(Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
            _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        
    }
}