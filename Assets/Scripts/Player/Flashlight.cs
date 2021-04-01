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
        public bool HasFlashlight { private set; get; }

        private float _intensity;
        private float _flickerTimer;
        private float _flickerTime;

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
            HasFlashlight = false;
            _light2D.intensity = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_light2D.enabled || !HasFlashlight) return;

            if (_flickerTimer >= _flickerTime)
                StartCoroutine(Flicker());
            _flickerTimer += Time.deltaTime;
        }
        
        public void SwitchLight()
        {
            if (!HasFlashlight) return;
            _audioController.GetComponent<SFX>().PlayClick();
            _light2D.enabled = !_light2D.enabled;
            _light2D.intensity = _intensity;
        }

        public void EquipFlashlight()
        {
            HasFlashlight = true;
        }
    }
}
