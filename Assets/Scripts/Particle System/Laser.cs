using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering.Universal;

namespace Particle_System
{
    public class Laser : MonoBehaviour
    {
        public float shootAfter;
        private LineRenderer _laser;
        private Light2D _pointLight;
        private float _startIntensity;
        private readonly float _lightFlickerSpeed = 15;

        private void Awake()
        {
            _laser = transform.GetComponentInChildren<LineRenderer>();
            _pointLight = transform.GetComponentInChildren<Light2D>();
            _startIntensity = _pointLight.intensity;
            _pointLight.intensity = 0;
        }

        private void Start()
        {
            StartCoroutine(ShootLaser());
        }

        private IEnumerator ShootLaser()
        {
            CameraEffects.Instance.ShakeCamera(.4f,shootAfter);
            yield return new WaitForSeconds(shootAfter);
            while (_pointLight.intensity < _startIntensity)
            {
                var increment = Time.deltaTime * _lightFlickerSpeed;
                _pointLight.intensity += increment;
                yield return new WaitForEndOfFrame();
            }
            CameraEffects.Instance.ShakeCamera(1,.1f);
            _laser.SetPosition(1, new Vector3( 20,0,0));
            while (_pointLight.intensity > 0)
            {
                var increment = Time.deltaTime * _lightFlickerSpeed;
                _pointLight.intensity -= increment;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(.5f);
            _laser.SetPosition(1, Vector3.zero);
        }
    }
}
