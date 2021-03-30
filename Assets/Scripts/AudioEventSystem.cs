using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventSystem : MonoBehaviour
{
    private GameObject _audioController;


    private bool isTriggered = false;

    public float startingPitch = 1;
    public float endingPitch = 0.35f;
    public float timeToDecrease = 5;
    // Start is called before the first frame update
    void Start()
    {
        _audioController = GameObject.Find("AudioController");

    }

    // Update is called once per frame
    void Update()
    {

        if (isTriggered && _audioController.GetComponent<SFX>().wind.pitch >= endingPitch)
        {
            _audioController.GetComponent<SFX>().wind.pitch -= Time.deltaTime * startingPitch / timeToDecrease;
        }
        if(_audioController.GetComponent<SFX>().wind.pitch == endingPitch)
        {
            isTriggered = false;
        }

        //if (isTriggered && _audioController.GetComponent<SFX>().wind.pitch < startingPitch)
        //{
        //    _audioController.GetComponent<SFX>().wind.pitch += Time.deltaTime * startingPitch / timeToDecrease;
        //}

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_audioController.GetComponent<SFX>().wind.pitch == startingPitch)
            isTriggered = true;
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if(collision.collider.CompareTag("SoundTrigger"))
    //    {

    //    }
    //}
}
