using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    private Light2D _light2D;

    public float maxRange = 1;
    public float minRange = .3f;
    public float flickerSpeed = 1;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        _light2D.intensity = Mathf.Lerp(minRange, maxRange, Mathf.PingPong(Time.time, flickerSpeed));
    }
}
