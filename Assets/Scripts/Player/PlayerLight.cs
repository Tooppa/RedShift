using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    private Light2D _light2D;

    public float totalSeconds;     // The total of seconds the flash wil last
    public float maxIntensity;     // The maximum intensity the flash will reach

    public IEnumerator flash()
    {
        float waitTime = totalSeconds / 2;
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (_light2D.intensity < maxIntensity)
        {
            _light2D.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (_light2D.intensity > 0)
        {
            _light2D.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return null;
        StartCoroutine(flash());
    }

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        StartCoroutine(flash());
    }
}
