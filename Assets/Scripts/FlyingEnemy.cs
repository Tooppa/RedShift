using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    public EnemyScriptable data;

    public Transform target;

    public float nextWaypointDistance = 1f;

    private Vector2 origin;

    private float spawnRadius;

    private float speed;
    private float jumpHeight;
    private float enemyRange;
    private float knockbackForce;
    private float flyingEnemyBounciness;

    private bool isTargetInRange = false;
    private bool isGrounded = false;
    private readonly Vector2 _groundCheckOffset = new Vector2(0, -0.5f);
    private const float GroundedRadius = 0.42f;
    [SerializeField] private LayerMask whatIsGround;

    Path path;
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        speed = data.speed;
        jumpHeight = data.jumpHeight;
        enemyRange = data.enemyRange;
        knockbackForce = data.knockbackForce;
        spawnRadius = data.spawnRadius;
        flyingEnemyBounciness = data.flyingEnemyBounciness;
    }
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);

        //Adds forward force to the enemy's jump
        if (!isGrounded)
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (!isTargetInRange && (distanceToPlayer <= -spawnRadius || distanceToPlayer >= spawnRadius))
        {
            transform.position = origin;
        }

        //Checks if player is out of enemyObject's range. If out of range, enemy stops moving.
        isTargetInRange = true;
        if (distanceToPlayer <= -enemyRange || distanceToPlayer >= enemyRange)
        {
            isTargetInRange = false;
            transform.position = Vector2.MoveTowards(transform.position, origin, 0.15f);
        }
    }

    private void Update()
    {
        PlayerHit();
    }

    private void PlayerHit()
    {
        float distanceX = target.transform.position.x - transform.position.x;
        float distanceY = target.transform.position.y - transform.position.y;
        if (distanceX <= knockbackForce && distanceX > -knockbackForce && distanceY <= knockbackForce && distanceY > -knockbackForce)
        {
            Debug.Log("Hit!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float bounciness = 0f;
        if (collision.collider.CompareTag("Player"))
        {
            bounciness = flyingEnemyBounciness;
        }
        rb.velocity += collision.relativeVelocity * bounciness;
    }
}
