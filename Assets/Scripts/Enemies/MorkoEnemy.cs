using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Seeker), typeof(Animator))]
public class MorkoEnemy : MonoBehaviour
{
    public EnemyScriptable data;

    private Transform _player;

    private Rigidbody2D _playerRigidbody2D;

    [SerializeField] private float nextWaypointDistance = 2.5f;
    
    private bool _isGrounded = false;
    private readonly Vector2 _groundCheckOffset = new Vector2(0, -2.335f);
    private const float GroundedRadius = 0.45f;
    [SerializeField] private LayerMask whatIsGround;

    private readonly Vector2 _legOffset = new Vector2(0,-2.3f);
    [SerializeField] private float distanceFromRaycast;

    private Path _path;
    private int _currentWaypoint = 0;

    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;
    private static readonly int Walk = Animator.StringToHash("Walk");

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"No scriptable object for {gameObject.name}");
            this.enabled = false;
            return;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;

        if (_player == null) 
            this.enabled = false;
        
        _playerRigidbody2D = _player.GetComponent<Rigidbody2D>();

        if (_playerRigidbody2D == null)
            this.enabled = false;
        
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.2f);
    }
    
    void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(_rigidbody2D.position, _player.transform.position, OnPathComplete);
    }
    
    void OnPathComplete(Path p)
    {
        if (p.error) return;
        
        _path = p;
        _currentWaypoint = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_path == null)
            return;
     
        var positionCache = transform.position;
        
        var distanceToPlayer = Vector2.Distance(positionCache, _player.position); // Always positive
        
        if(distanceToPlayer > data.enemyRange)
            return;

        _animator.SetTrigger(Walk);
        
        if (_currentWaypoint < _path.vectorPath.Count - 1)
        {
            var distanceToWaypoint = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }
        
        // Correct waypoint selected, move towards the waypoint

        Vector2 enemyToWaypoint = ((Vector2)_path.vectorPath[_currentWaypoint] - (Vector2) positionCache).normalized;
        
        Vector2 force = enemyToWaypoint * data.speed;
        
        _rigidbody2D.AddForce(force);
        
        // Flip the sprite around if direction changes
        var transformCache = transform;
        transformCache.localScale = new Vector3(1 * Mathf.Sign(force.x), 1, 1);

        // Check if there is a need to jump
        // Determine that by raycasting from the legs
        var legPosition = (Vector2) positionCache + _legOffset;

        // Raycast from the legs by the specified length. Only collide with Ground
        var hit = Physics2D.Raycast(legPosition, Vector3.right * transformCache.localScale.x, distanceFromRaycast, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            Debug.DrawRay(legPosition, Vector3.right * (distanceFromRaycast * transformCache.localScale.x), Color.red);
            _rigidbody2D.AddForce(Vector2.up * 18, ForceMode2D.Impulse);
        }
        else
        {
            Debug.DrawRay(legPosition, Vector3.right * (distanceFromRaycast * transformCache.localScale.x), Color.gray);
        }

    }

    private void Update()
    {
        CheckIsGrounded();
        PlayerHit();
    }

    private void CheckIsGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, GroundedRadius, whatIsGround);
    }

    private void PlayerHit()
    {
        var playerPosition = _player.position;
        var enemyPosition = transform.position;
        
        float distanceX = playerPosition.x - enemyPosition.x;
        float distanceY = playerPosition.y - enemyPosition.y;
        
        if (distanceX <= data.knockbackRadius && distanceX > -data.knockbackRadius && distanceY <= data.knockbackRadius && distanceY > -data.knockbackRadius)
        {
            Debug.Log("Hit!");
            PlayerPushback();
        }
    }

    private void PlayerPushback()
    {
        Vector2 knockbackDirection = (_player.position - transform.position).normalized;
        _playerRigidbody2D.AddForce(knockbackDirection * data.knockbackForce);
    }
}
