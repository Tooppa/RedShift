using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXIntervalandPitchRandomizer : MonoBehaviour
{
    public AudioSource _audio;

    private SFXPlayCounter counter;

    public int possibleStartTime;
    public int possibleEndTime;
    public float pitchLowerBorder;
    public float pitchHigherBorder;

    private int sum;

    private int rand;

    private void Start()
    {
        counter = GameObject.Find("SFXPlayCounter").GetComponent<SFXPlayCounter>();
        rand = Random.Range(possibleStartTime, possibleEndTime);
        sum = rand;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (counter.counter >= sum)
        {
            _audio.pitch = (float)Random.Range(pitchLowerBorder, pitchHigherBorder);
            _audio.Play();
            rand = Random.Range(possibleStartTime, possibleEndTime);
            sum += rand;
        }
    }
}
