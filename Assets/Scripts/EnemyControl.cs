using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    private Vector2 playerDetectionArea;
    private Rigidbody2D rb2D;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IdleWalk();
    }

    void IdleWalk()
    {
        rb2D.AddForce(Vector2.left * speed * Time.deltaTime, ForceMode2D.Impulse);
    }
}
