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
        private Rigidbody2D _rigidbody2D;
    
        private readonly Vector2 _groundCheckOffset = new Vector2(0,-0.5f);

        private bool _isGrounded = false;
        public bool HasRocketBoots { private set; get; }
        private bool _rocketBootsCooldown = false;
        private bool runningSoundOnCooldown;
        private bool isJumping = false;
        private bool musicPlaying = false;
        private Animator _animator;
        private const float GroundedRadius = 0.3f;

        private Vector2 playerStartAltitude;
        private Vector2 playerEndAltitude;

        private GameObject _gun;

        private SFX _audioController;

        public float runningSoundCooldownTime;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _gun = GameObject.Find("Gun");
            HasRocketBoots = false;
        }

        private void Start()
        {
            playerStartAltitude = transform.position;
            _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
            rocketBoots.Stop();
        }

        private void Update()
        {
            CheckIsGrounded();
            /* music is disabled since it used old inputs
            if (Input.GetKeyDown(KeyCode.G) && !musicPlaying)
            {
                _audioController.GetComponent<SFX>().PlayCalmAmbience();
                musicPlaying = true;
            }
            else if(Input.GetKeyDown(KeyCode.G) && musicPlaying)
            {
                _audioController.GetComponent<SFX>().calmAmbience.Pause();
                musicPlaying = false;
            }
            */
        }
        private void CheckIsGrounded()
        {
            _isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, GroundedRadius, whatIsGround);
            switch (_isGrounded)
            {
                case false:
                    playerEndAltitude = transform.position;
                    isJumping = true;
                    break;
                case true when isJumping && (playerStartAltitude.y - playerEndAltitude.y) > 1:
                    _audioController.PlayLanding();
                    isJumping = false;
                    break;
                default:
                    playerStartAltitude = transform.position;
                    break;
            }
        }
        
        public void Movement(Vector2 move)
        {
            var inputDirection = move.x;
            if (inputDirection != 0)
            {
                transform.localScale = new Vector3(inputDirection, 1, 1);
                _gun.transform.localScale = new Vector3(inputDirection, 1, 1);
                CameraEffects.Instance.ChangeOffset(.3f ,inputDirection * 2);
                _animator.SetBool("Walking", true);

                if(_isGrounded && !runningSoundOnCooldown)
                {
                    _audioController.PlayRunning();
                    StartCoroutine(RunningSoundCooldown());
                }
            }
            else
            {
                _animator.SetBool("Walking", false);
            }

            if (Mathf.Abs(_rigidbody2D.velocity.x) < maxSpeed)
                _rigidbody2D.AddForce(Vector2.right * (inputDirection * speed * Time.deltaTime), ForceMode2D.Impulse);

            _animator.SetBool("Jumping", !_isGrounded);
        }

        public void Jump()
        {
            if (!_isGrounded || Time.timeScale != 1) return;
            _animator.SetTrigger("TakeOff");
            _rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        public void Dash()
        {
            if (HasRocketBoots && _rocketBootsCooldown && Time.timeScale == 1) StartCoroutine(IEDash());
        }

        public void EquipRocketBoots()
        {
            HasRocketBoots = true;
        }

        private IEnumerator IEDash(){
            StartCoroutine(Cooldown(.6f));
            _rigidbody2D.AddForce(Vector2.right  * (transform.localScale.x * rocketBootsSpeed), ForceMode2D.Impulse);
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
        private IEnumerator RunningSoundCooldown()
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            runningSoundOnCooldown = true;
            yield return new WaitForSeconds(runningSoundCooldownTime);
            runningSoundOnCooldown = false;
        }
    }
}