using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    [RequireComponent(typeof(Light2D))]
    public class Flashlight : MonoBehaviour
    {
        private Light2D _light2D;
        private GameObject _audioController;

        public float cooldownTime;
        private bool _inCooldown;

        private float _intensity;
        private float _flickerTimer;
        private float _flickerTime;

        private IEnumerator Cooldown()
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            _inCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _inCooldown = false;
        }

        private IEnumerator Flicker()
        {
            //Turn the flashlight off, wait for the cooldown time to pass, then turn the flashlight back on
            _light2D.intensity = 0.1f;
            yield return new WaitForSeconds(.1f);
            _light2D.intensity = _intensity;
            yield return new WaitForSeconds(.2f);
            _light2D.intensity = 0.1f;
            yield return new WaitForSeconds(Random.Range(0, 2));
            _light2D.intensity = _intensity;
            _flickerTimer = 0;
        }

        private void Start()
        {
            _light2D = GetComponent<Light2D>();
            _audioController = GameObject.Find("AudioController");
            _flickerTime = 2;
            _intensity = _light2D.intensity;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_light2D.enabled) return;

            if (_flickerTimer >= _flickerTime)
                StartCoroutine(Flicker());
            _flickerTimer += Time.deltaTime;
        }
        
        public void SwitchLight()
        {
            if (_inCooldown) return;
            StartCoroutine(Cooldown());
            _audioController.GetComponent<SFX>().PlayClick();
            _light2D.enabled = !_light2D.enabled;
            _light2D.intensity = _intensity;

            StartCoroutine(Cooldown());
        }
    }
}
