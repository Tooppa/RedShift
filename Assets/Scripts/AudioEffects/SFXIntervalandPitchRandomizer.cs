using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXIntervalandPitchRandomizer : MonoBehaviour
{
    public AudioSource _audio;

    private float counter;

    public int possibleStartTime;
    public int possibleEndTime;
    public float pitchLowerBorder;
    public float pitchHigherBorder;

    private int rand;

    private void Start()
    {
        rand = Random.Range(possibleStartTime, possibleEndTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;
        if (counter >= rand)
        {
            _audio.pitch = (float)Random.Range(pitchLowerBorder, pitchHigherBorder);
            _audio.Play();
            counter = 0;
            rand = Random.Range(possibleStartTime, possibleEndTime);
        }
    }
}
