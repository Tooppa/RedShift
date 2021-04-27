using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SFXIntervalandPitchRandomizer : MonoBehaviour
{
    public AudioSource[] _audio;

    private float counter;

    public int possibleStartTime;
    public int possibleEndTime;
    public float pitchLowerBorder;
    public float pitchHigherBorder;

    private int rand;
    private int randomSound;

    private void Start()
    {
        rand = Random.Range(possibleStartTime, possibleEndTime);
        randomSound = Random.Range(0, _audio.Length);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        if (counter >= rand)
        {
            _audio[randomSound].pitch = (float)Random.Range(pitchLowerBorder, pitchHigherBorder);
            _audio[randomSound].Play();
            rand = Random.Range(possibleStartTime, possibleEndTime);
            randomSound = Random.Range(0, _audio.Length);
            counter = 0;
        }
    }
}
