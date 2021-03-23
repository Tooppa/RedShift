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
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float maxSpeed;
    private Rigidbody2D _rigidbody2D;
    
    private readonly Vector2 _groundCheckOffset = new Vector2(0,-0.5f);

    public bool _isGrounded = false;
    private Animator _animator;
    private const float GroundedRadius = 0.3f;

    
    public bool jumping;
    public float jumpTimeCounter;
    public float jumpTime;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        CheckIsGrounded();
        Movement();
    }
    private void CheckIsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + _groundCheckOffset, GroundedRadius, whatIsGround);
    }
    
    private void Movement()
    {
        var inputDirection = Input.GetAxisRaw("Horizontal");

        if (inputDirection != 0)
        {
            transform.localScale = new Vector3(inputDirection, 1, 1);
            CameraEffects.Instance.ChangeOffset(.3f ,inputDirection * 2);
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }

        if (Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
            _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("TakeOff");
            jumping = true;
            jumpTimeCounter = jumpTime;
            _rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        if (_isGrounded)
            _animator.SetBool("Jumping", false);
        else
            _animator.SetBool("Jumping", true);

        if (Input.GetKey(KeyCode.Space) && jumping == true)
        {
            if (jumpTimeCounter > 0)
                jumpTimeCounter += Time.deltaTime;
            else
                jumping = false;
        }
    }
}