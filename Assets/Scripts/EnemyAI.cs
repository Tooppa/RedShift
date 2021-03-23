using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed;
    public float jumpHeight;
    public float nextWaypointDistance = 1f;
    public float enemyRange;

    private Vector2 origin;

    private bool isTargetInRange = false;
    


    private bool isGrounded = false;
    private readonly Vector2 _groundCheckOffset = new Vector2(0, -0.5f);
    private const float GroundedRadius = 0.4f;
    [SerializeField] private LayerMask whatIsGround;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public Transform enemyGFX;

    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
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

    void SlimeHop()
    {
        //Vector2 force = new Vector2(1, 0) * speed * Time.deltaTime;
        //rb.AddForce(force);
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
        //Vector2 currentPos = transform.position;
        //if ((currentPos.x - origin.x) >= 1)
        //{
        //    target.position = origin;
        //}
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
