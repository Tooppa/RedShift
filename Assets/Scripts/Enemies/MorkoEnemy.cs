using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Seeker))]
public class MorkoEnemy : MonoBehaviour
{
    public EnemyScriptable data;

    private Transform _player;

    private Rigidbody2D _playerRigidbody2D;

    public float nextWaypointDistance = 1f;

    private Vector2 origin;

    private float _speed;
    private float _jumpHeight;
    private float _enemyRange;
    private float _knockbackForce;
    private float _knockbackRadius;

    private bool _isGrounded = false;
    private readonly Vector2 _groundCheckOffset = new Vector2(0, -2.335f);
    private const float GroundedRadius = 0.45f;
    [SerializeField] private LayerMask whatIsGround;

    private Path _path;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;

    private Seeker _seeker;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogWarning($"No scriptable object for {gameObject.name}");
            this.enabled = false;
            return;
        }
        
        _speed = data.speed;
        _jumpHeight = data.jumpHeight;
        _enemyRange = data.enemyRange;
        _knockbackForce = data.knockbackForce;
        _knockbackRadius = data.knockbackRadius;
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
        
        origin = transform.position;
        _seeker = GetComponent<Seeker>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
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
        
        // Don't increment waypoint if approaching the array bounds
        if (_currentWaypoint + 2 < _path.vectorPath.Count)
        {
            float distanceToWaypoint = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }
        
        var distanceToPlayer = Vector2.Distance(transform.position, _player.position); // Always positive
        
        if(distanceToPlayer > _enemyRange)
            return;
        
        // Correct waypoint selected, move towards the waypoint
        
        Vector2 enemyToWaypoint = ((Vector2)_path.vectorPath[_currentWaypoint] - _rigidbody2D.position).normalized;
        
        Vector2 force = enemyToWaypoint * _speed;
        
        _rigidbody2D.AddForce(force);
        
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
        float distanceX = _player.position.x - transform.position.x;
        float distanceY = _player.position.y - transform.position.y;
        if (distanceX <= _knockbackRadius && distanceX > -_knockbackRadius && distanceY <= _knockbackRadius && distanceY > -_knockbackRadius)
        {
            Debug.Log("Hit!");
            PlayerPushback();
        }
    }

    private void PlayerPushback()
    {
        Vector2 knockbackDirection = (_player.position - transform.position).normalized;
        _playerRigidbody2D.AddForce(knockbackDirection * _knockbackForce);
    }
}
