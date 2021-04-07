using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;

    public AudioSource _audio;
    public bool loop;
    public bool destroyOnTrigger;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            _audio.Play();
            if(loop)
            {
                _audio.loop = true;
            }
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
