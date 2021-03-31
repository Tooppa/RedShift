using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem _ps;
    public List<ParticleCollisionEvent> collisionEvents;
    public CinemachineVirtualCamera cam;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _ps.GetCollisionEvents(other, collisionEvents);
        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
        Vector3 direction = other.transform.position - transform.position;

        if (other.GetComponent<Rigidbody2D>() != null && 
            (other.GetComponent<Breakable>() != null || other.GetComponent<Piece>()) )
        {
            float time = 0;
            while(time < 0.5f)
            {
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(direction.normalized * 30, collisionEvents[0].intersection);
                time += Time.deltaTime;
            }
        }
        

        if (other.TryGetComponent(out Health health))
            health.TakeDamage(20);
    }
}
