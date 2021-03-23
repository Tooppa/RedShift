using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class JumpingSlimeAI : MonoBehaviour
{
    public EnemyScriptable data;

    public Transform target;

    public float nextWaypointDistance = 1f;

    private float speed;
    private float jumpHeight;
    private float enemyRange;

    private bool isTargetInRange = false;
    private bool isGrounded = false;
    private readonly Vector2 _groundCheckOffset = new Vector2(0, -0.5f);
    private const float GroundedRadius = 0.42f;
    [SerializeField] private LayerMask whatIsGround;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public Transform enemyGFX;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        speed = data.speed;
        jumpHeight = data.jumpHeight;
        enemyRange = data.enemyRange;
    }
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        InvokeRepeating("SlimeHop", 0f, 2f);

    }
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    //Function invoked every 2 seconds. Adds upwards force to the enemy.
    void SlimeHop()
    {
        if(isTargetInRange && isGrounded)
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
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
        Vector2 force = direction * speed * Time.deltaTime;

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
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }

        //Checks if player is out of enemyObject's range. If out of range, enemy stops moving.
        isTargetInRange = true;
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if(distanceToPlayer <= -enemyRange || distanceToPlayer >= enemyRange)
        {
            isTargetInRange = false;
        }
    }

    private void Update()
    {
        CheckIsGrounded();
        PlayerHit();
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, GroundedRadius, whatIsGround);
    }

    private void PlayerHit()
    {
        float distanceX = target.transform.position.x - transform.position.x;
        float distanceY = target.transform.position.y - transform.position.y;
        if (distanceX <= 0.8 && distanceX > -0.8 && distanceY <= 0.8 && distanceY > -0.8)
        {
            Debug.Log("Hit!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
    }
}
