using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

#pragma warning disable 0414

public class JumpingSlimeAI : MonoBehaviour
{
    public EnemyScriptable data;

    public Transform target;

    public Rigidbody2D playerRB;

    private Animator _animator;
    private Health _playerHealth;
    private Health _health;
    private BoxCollider2D _collider;

    private CaterpillarSFX _caterpillarSFX;

    public float nextWaypointDistance = 1f;

    private Vector2 origin;

    private float speed;
    private float enemyRange;
    private float knockbackForce;
    private float knockbackRadius;
    private float spawnRadius;

    private bool deadPillar = false;
    private bool _cooldown;
    private bool canMove = true;
    private bool isTargetInRange = false;
    [SerializeField] private LayerMask whatIsGround;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        speed = data.speed;
        enemyRange = data.enemyRange;
        knockbackForce = data.knockbackForce;
        knockbackRadius = data.knockbackRadius;
        spawnRadius = data.spawnRadius;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRB = playerRB.GetComponent<Rigidbody2D>();
        origin = transform.position;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("Crawling", true);
        _playerHealth = target.GetComponentInParent<Health>();
        _health = GetComponent<Health>();
        _caterpillarSFX = GetComponentInChildren<CaterpillarSFX>();
        _caterpillarSFX.PlayIdle();

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

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
        if (!isTargetInRange && (distanceToPlayer <= -spawnRadius || distanceToPlayer >= spawnRadius))
        {
            transform.position = origin;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);

        //Adds forward force to the enemy with a cooldown
        if (canMove)
        {
            rb.AddForce(force);
            StartCoroutine(MoveCoolDown());
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //Checks if player is out of enemyObject's range. If out of range, enemy stops moving.
        isTargetInRange = true;
        if(distanceToPlayer <= -enemyRange || distanceToPlayer >= enemyRange)
        {
            isTargetInRange = false;
            transform.position = Vector2.MoveTowards(transform.position, origin, 0.15f);
        }
    }

    private void Update()
    {

        if(!deadPillar)
            PlayerHit();

        if(_health.CurrentHealth <= 0 && !deadPillar)
        {
            deadPillar = true;
            _collider.enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    private void PlayerHit()
    {
        float distanceX = target.transform.position.x - transform.position.x;
        float distanceY = target.transform.position.y - (transform.position.y - 1);
        if (distanceX <= knockbackRadius && distanceX > -knockbackRadius && distanceY <= knockbackRadius && distanceY > -knockbackRadius && !_cooldown)
        {
            Debug.Log("Hit!");
            _animator.SetTrigger("Attack");
            _caterpillarSFX.PlayAttack();
            PlayerPushback();
            _playerHealth.TakeDamage(33);
            StartCoroutine(DamageCooldown());
        }
    }

    void PlayerPushback()
    {
        Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
        playerRB.AddForce(knockbackDirection * knockbackForce);
    }

    private IEnumerator MoveCoolDown()
    {
        canMove = false;
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    private IEnumerator DamageCooldown()
    {
        _cooldown = true;
        yield return new WaitForSeconds(1);
        _cooldown = false;
    }

    private IEnumerator DeathAnimation()
    {
        _animator.SetBool("Crawling", false);
        _animator.SetTrigger("Death");
        _caterpillarSFX.PlayDeath();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !deadPillar)
        {
            PlayerPushback();
        }
    }
}
