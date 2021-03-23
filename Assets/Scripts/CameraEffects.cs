using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    // kuinka käytetetään effectejä
    // cameraShake toimii kutsumalla instancen kautta seuraavasti 
    // CameraEffects.Instance.ShakeCamera(5, .1f); ensimmäinen on intensiteetti ja toinen aika
    // 
    // change offsett toimii asettamalla x akselin arvon
    // esim. CameraEffects.Instance.ChangeOffset(-2);
    public static CameraEffects Instance { get; private set; }
    private CinemachineVirtualCamera _cam;
    private CinemachineFramingTransposer _transposer;
    private void Awake()
    {
        Instance = this;
        _cam = GetComponent<CinemachineVirtualCamera>();
        _transposer = _cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        StartCoroutine(SmoothShake(intensity, time));
    }
    private IEnumerator SmoothShake(float intensity, float time)
    {
        var perlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(time);
        perlin.m_AmplitudeGain = 0f;
    }

    public void ChangeOffset(float x)
    {
        StartCoroutine(Offset(0.3f, _transposer.m_TrackedObjectOffset.x, x));
    }

    private IEnumerator Offset(float timer, float start, float stop)
    {
        float counter = 0;
        while (counter < timer)
        {
            counter += Time.deltaTime;
            var newX = Mathf.Lerp(start, stop, counter / timer);
            _transposer.m_TrackedObjectOffset = new Vector3(newX, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }
}