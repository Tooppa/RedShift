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

    private GameObject player;

    private GameObject _audioController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        _audioController = GameObject.Find("AudioController");
    }


    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _ps.GetCollisionEvents(other, collisionEvents);

        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
        //_audioController.GetComponent<SFX>().PlayProjectileCollisionExplosion();

        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        var pmain = p.main;

        if (other.GetComponent<Rigidbody2D>() != null)
            other.GetComponent<Rigidbody2D>().AddForceAtPosition(collisionEvents[0].intersection * 10 - transform.position, collisionEvents[0].intersection);

        if (other.TryGetComponent(out Health health))
            health.TakeDamage(100);
    }
}
