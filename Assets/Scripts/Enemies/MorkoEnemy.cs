using System.Collections;
using UnityEngine;

public class MorkoEnemy : MonoBehaviour
{
    public EnemyScriptable data;

    private Transform _player;
    private Rigidbody2D _playerRigidbody2D;
    private Health _playerHealth;

    private MorkoSFXController morkoSFX;
    
    // Mörkö tries to find and climb over obstacles of this height
    private readonly Vector2 _footOffset = new Vector2(0,-2.3f);
    
    [SerializeField] private float distanceToTriggerJump;

    private Rigidbody2D _rigidbody2D;

    private Animator _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");

    private float cooldownTime = 2f;

    private readonly float attackCooldown = 1f;
    private bool _canAttack = true;

    private bool isMorkoAwake = false;
    private bool _screamSoundOnCooldown = true;

    public AudioSource[] morkoSteps;
    public AudioSource morkoScream;

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"No scriptable object for {gameObject.name}");
            this.enabled = false;
        }       
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        morkoSFX = GameObject.Find("MorkoSFXController").GetComponent<MorkoSFXController>();

        if (_player == null) 
            this.enabled = false;
        
        _playerRigidbody2D = _player.GetComponent<Rigidbody2D>();

        if (_playerRigidbody2D == null)
            this.enabled = false;

        _playerHealth = _player.GetComponent<Health>();

        if (_playerHealth == null)
            this.enabled = false;
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (this.isActiveAndEnabled && !isMorkoAwake)
        {
            morkoSFX.PlayMorkoScream();
            isMorkoAwake = true;
            morkoSFX.PlayMorkoGrowl();
        }         

        var positionCache = transform.position;
        
        var distanceToPlayer = Vector2.Distance(positionCache, _player.position); // Always positive
        
        if(distanceToPlayer > data.enemyRange)
            return;
        
        _animator.SetTrigger(Walk);
        
        // Move towards the player

        Vector2 enemyToPlayer = ((Vector2)_player.position - (Vector2) positionCache).normalized;
        
        Vector2 force = enemyToPlayer * data.speed;
        
        _rigidbody2D.AddForce(force);
        
        // Flip the sprite around if direction changes
        var transformCache = transform;
        transformCache.localScale = new Vector3(1 * Mathf.Sign(force.x), 1, 1);
        
        var localScaleCache = transformCache.localScale;

        // Check if there is a need to climb
        // Determine that by raycasting on the leg level
        var footPosition = (Vector2) positionCache + _footOffset;
        
        var footPositionInRaycast = footPosition + new Vector2(localScaleCache.x * distanceToTriggerJump ,0);
        var bodyPositionInRaycast = (Vector2) positionCache + new Vector2(localScaleCache.x * distanceToTriggerJump ,0);

        // Raycast from the legs by the specified length. Only collide with Ground
        // Raycast from feet to the knee from distanceToTriggerJump
        var hitFromFeetToLegs = Physics2D.Linecast(footPositionInRaycast, bodyPositionInRaycast, LayerMask.GetMask("Ground"));

        if (hitFromFeetToLegs.collider != null)
        {
            _rigidbody2D.AddForce(Vector2.up * data.jumpHeight, ForceMode2D.Impulse);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if(_canAttack) Attack();
    }

    private void Attack()
    {
        PushBack();

        _playerHealth.TakeDamage(data.attack);

        StartCoroutine(AttackCooldown());
    } 

    private void PushBack()
    {
        // Direction always from the enemy to the player
        _playerRigidbody2D.AddForce((_player.position - transform.position).normalized * data.knockbackForce, ForceMode2D.Impulse);
    }


}
