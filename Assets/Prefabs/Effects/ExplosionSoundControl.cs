using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundControl : MonoBehaviour
{
    public float explosionHeardDistance;

    private AudioSource explosion;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        explosion = GetComponentInChildren<AudioSource>();
        player = GameObject.Find("Player");
        explosion.Play();
    }
}
