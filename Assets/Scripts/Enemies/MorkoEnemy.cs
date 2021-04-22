using UnityEngine;

public class MorkoEnemy : MonoBehaviour
{
    public EnemyScriptable data;

    private Transform _player;

    private Rigidbody2D _playerRigidbody2D;
    
    private readonly Vector2 _legOffset = new Vector2(0,-2.3f);
    [SerializeField] private float distanceToTriggerJump;

    private Rigidbody2D _rigidbody2D;

    private Animator _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");

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

        if (_player == null) 
            this.enabled = false;
        
        _playerRigidbody2D = _player.GetComponent<Rigidbody2D>();

        if (_playerRigidbody2D == null)
            this.enabled = false;
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        var positionCache = transform.position;
        
        var distanceToPlayer = Vector2.Distance(positionCache, _player.position); // Always positive
        
        if(distanceToPlayer > data.enemyRange)
            return;
        
        if(distanceToPlayer < data.knockbackRadius)
            Attack();

        _animator.SetTrigger(Walk);
        
        // Move towards the player

        Vector2 enemyToPlayer = ((Vector2)_player.position - (Vector2) positionCache).normalized;
        
        Vector2 force = enemyToPlayer * data.speed;
        
        _rigidbody2D.AddForce(force);
        
        // Flip the sprite around if direction changes
        var transformCache = transform;
        transformCache.localScale = new Vector3(1 * Mathf.Sign(force.x), 1, 1);

        // Check if there is a need to jump
        // Determine that by raycasting from the legs
        var legPosition = (Vector2) positionCache + _legOffset;

        // Raycast from the legs by the specified length. Only collide with Ground
        var hit = Physics2D.Raycast(legPosition, Vector3.right * transformCache.localScale.x, distanceToTriggerJump, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            _rigidbody2D.AddForce(Vector2.up * 18, ForceMode2D.Impulse);
        }

    }
    
    private void Attack() => PushBack();

    private void PushBack()
    {
        // Direction always from the enemy to the player
        _playerRigidbody2D.AddForce((_player.position - transform.position) * data.knockbackForce);
    }
}
