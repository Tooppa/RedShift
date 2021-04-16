using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float rocketBootsSpeed;
        [SerializeField] private ParticleSystem rocketBoots;
        [SerializeField] private float coyoteTime;
        private Rigidbody2D _rigidbody2D;

        private readonly Vector2 _groundCheckOffset = new Vector2(0, -0.5f);

        private bool _isGrounded;
        private bool _holdingJump;
        public bool HasRocketBoots { private set; get; }
        private bool _rocketBootsCooldown;

        private Animator _animator;
        private const float GroundedRadius = 0.3f;
        private float _timer;

        [SerializeField] private float holdingJumpTime = 0;
        [SerializeField] private float holdingJumpTimeMax = 0.2f;

        private GameObject _gun;
        private GameObject _audioController;

        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int TakeOff = Animator.StringToHash("TakeOff");
        private static readonly int Landing = Animator.StringToHash("Landing");

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = transform.GetChild(1).GetComponent<Animator>();
            _gun = GameObject.Find("Gun");
            _audioController = GameObject.Find("AudioController");
            HasRocketBoots = false;
        }

        private void Start()
        {
            rocketBoots.Stop();
        }

        private void Update()
        {
            CheckIsGrounded();
        }

        private void CheckIsGrounded()
        {
            var beforeGroundedCheck = _isGrounded;
            _isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, GroundedRadius, whatIsGround);
            _animator.SetBool(Landing, _rigidbody2D.velocity.y < -0.1f);
            if (beforeGroundedCheck && !_isGrounded)
                _timer = 0;
            _timer += Time.deltaTime;
        }

        public void Movement(Vector2 move)
        {
            var inputDirection = Mathf.Round(move.x);
            if (inputDirection != 0)
            {
                transform.localScale = new Vector3(inputDirection, 1, 1);
                _gun.transform.localScale = new Vector3(inputDirection, 1, 1);
                CameraEffects.Instance.ChangeOffset(.3f, inputDirection * 2);
                _animator.SetBool(Walking, true);
                rocketBoots.gameObject.transform.localScale = new Vector3(inputDirection, 1, 1);
            }
            else
            {
                _animator.SetBool(Walking, false);
            }

            if (Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
                _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);
        }

        public void Jump(float value)
        {
            if (value > 0)
            {
                if (_timer < coyoteTime) _isGrounded = true;
                if (!_isGrounded || Time.timeScale != 1) return;
                _holdingJump = true;
                _animator.SetTrigger(TakeOff);
                _rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                return;
            }
            _holdingJump = false;
            holdingJumpTime = 0;
        }

        private void FixedUpdate()
        {
            if (_holdingJump && holdingJumpTime < holdingJumpTimeMax)
            {
                _rigidbody2D.AddForce(Vector2.up * (2 * (Mathf.Pow((holdingJumpTime + 1) * 5, 2))), ForceMode2D.Impulse);
                holdingJumpTime += Time.deltaTime;
            }
        }

        public void Dash()
        {
            if (HasRocketBoots && !_rocketBootsCooldown && Time.timeScale == 1)
            {
                _audioController.GetComponent<SFX>().PlayPlayerRocketDash();
                StartCoroutine(IEDash());
            }

        }

        public void EquipRocketBoots()
        {
            _audioController.GetComponent<SFX>().PlayPowerUp();
            HasRocketBoots = true;
        }

        private IEnumerator IEDash()
        {
            StartCoroutine(Cooldown(.6f));
            _rigidbody2D.AddForce(Vector2.right * (transform.localScale.x * rocketBootsSpeed), ForceMode2D.Impulse);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.gravityScale = .1f;
            rocketBoots.Play();

            yield return new WaitForSeconds(0.2f);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x * 0.5f, 0f);
            _rigidbody2D.gravityScale = 1;
            rocketBoots.Stop();
        }
        private IEnumerator Cooldown(float cooldownTime)
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            _rocketBootsCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _rocketBootsCooldown = false;
        }
    }
}