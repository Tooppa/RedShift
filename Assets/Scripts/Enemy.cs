using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy variables
    Rigidbody2D rb;

    public int health;
    public int maxHealth;

    public float moveSpeed;
    public Transform movePoint;
    Vector2 movement;
    public Animator animator;

    public LayerMask whatStopsMovement;
    public float direction;

    public float counter;
    public float maxCounter;
    // End of enemy variables

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        movePoint.parent = null; // Detach the movePoint from the enemy so it is independent and does not move when the enemy moves
    }

    private void Update()
    {
        // Moves the enemy towards the movePoint at all times. MovePoint is moved in the Move()-function
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f && counter >= maxCounter)
        {
            // Random number (1/-1) for dictating if the enemy should move left or right
            direction = Random.Range(0, 2) * 2 - 1;

            Move();
        }

        // Move the time counter. More info in the Move()-function
        counter += Time.deltaTime;
    } // END OF UPDATE


    public void Move()
    {
        // Tell the animator which direction the enemy will move so the animator can start playing the correct animation
        animator.SetFloat("Horizontal", direction);

        // Check that there's no obstacle in the way
        if (counter >= maxCounter && !Physics2D.OverlapCircle(movePoint.position + new Vector3(direction * 0.5f, 0f, 0f), .05f, whatStopsMovement))
        {
            movePoint.position += new Vector3(direction * 0.5f, 0f, 0f); // Move the movePoint by 1/-1 horizontally
            counter = 0;
        }
        // Counter is for making sure that the movePoint does not move until the enemy has reached the previous movePoint position

        // Update animator attributes to match what the sprite is doing
        animator.SetFloat("Speed", movement.normalized.sqrMagnitude);
        animator.SetFloat("LastHorizontal", movement.x); // The last horizontal direction is saved for the animator's Idle-state
        animator.SetBool("Idle", true); // After moving, the animator will be set to Idle until Move() is called again
    } // End of Update

    // Collision detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser")) // Laser hit
        {
            TakeDamage(1);
        }
    }

    // TakeDamage function for reducing enemy health
    public void TakeDamage(int dmgAmount)
    {
        health -= dmgAmount;

        if (health <= 0)
        {
            // The enemy dies and is destroyed
            Destroy(gameObject);
        }
    }
}
