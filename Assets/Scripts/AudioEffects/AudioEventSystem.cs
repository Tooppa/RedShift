using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventSystem : MonoBehaviour
{
    private GameObject _audioController;
    private GameObject player;

    private Collider2D windPitchChangerColl;

    private bool upIsTriggered = false;
    private bool downIsTriggered = false;

    public float startingPitch = 1;
    public float endingPitch = 0.35f;
    public float timeToDecrease = 5;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {

        if (downIsTriggered)
        {
            _audioController.GetComponent<SFX>().wind.pitch -= Time.deltaTime * startingPitch / timeToDecrease;
        }
        if(_audioController.GetComponent<SFX>().wind.pitch <= endingPitch + 0.02)
        {
            downIsTriggered = false;
            _audioController.GetComponent<SFX>().wind.pitch = 0.35f;
        }

        if (upIsTriggered)
        {
            _audioController.GetComponent<SFX>().wind.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        }
        if (_audioController.GetComponent<SFX>().wind.pitch >= startingPitch - 0.02)
        {
            upIsTriggered = false;
            _audioController.GetComponent<SFX>().wind.pitch = 1;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && _audioController.GetComponent<SFX>().wind.pitch == startingPitch)
        {
            downIsTriggered = true;
        }

        if (other.gameObject == player && _audioController.GetComponent<SFX>().wind.pitch == endingPitch)
        {
            upIsTriggered = true;
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if(collision.collider.CompareTag("SoundTrigger"))
    //    {

    //    }
    //}
}
