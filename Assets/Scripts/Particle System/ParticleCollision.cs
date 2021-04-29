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

    public bool weakShot;

    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        var breakable = other.TryGetComponent(out Breakable _);
        if (other.TryGetComponent(out Health health) && !(weakShot && breakable))
            health.TakeDamage(20);
        
        var numCollisionEvents = _ps.GetCollisionEvents(other, collisionEvents);
        var explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
        var direction = other.transform.position - transform.position;

        if (other.TryGetComponent(out Rigidbody2D component) && 
            (breakable || other.TryGetComponent(out Piece piece)))
            component.AddForceAtPosition(direction.normalized * 200, collisionEvents[0].intersection, ForceMode2D.Impulse);
        
    }
}
