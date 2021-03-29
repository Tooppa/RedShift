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
        private Rigidbody2D _rigidbody2D;
    
        private readonly Vector2 _groundCheckOffset = new Vector2(0,-0.5f);

        public bool _isGrounded = false;
        private Animator _animator;
        private const float GroundedRadius = 0.3f;

        private GameObject _gun;

        private GameObject _audioController;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _gun = GameObject.Find("Gun");
        }

        private void Start()
        {
            _audioController = GameObject.Find("AudioController");
        }

        private void Update()
        {
            CheckIsGrounded();
            Movement();

            if(Input.GetKeyDown(KeyCode.E))
            {
                _audioController.GetComponent<SFX>().PlayCalmAmbience();
            }
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
                _gun.transform.localScale = new Vector3(inputDirection, 1, 1);
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
                _rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }

            _animator.SetBool("Jumping", !_isGrounded);
        }
    }
}