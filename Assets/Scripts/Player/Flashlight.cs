using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

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
        private Vector2 _mousePos;
        private Camera _main;
        private Transform _player;
        private float _angle;

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
            _main = Camera.main;
            _player = transform.parent.GetComponent<Transform>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_light2D.enabled || !HasFlashlight) return;

            if (_flickerTimer >= _flickerTime)
                StartCoroutine(Flicker());
            _flickerTimer += Time.deltaTime;
            PointFlashlight();
        }

        private void PointFlashlight()
        {
            if (!HasFlashlight) return;
            _mousePos = Mouse.current.position.ReadValue();
            var objectPos = _main.WorldToScreenPoint(
                _player.position + new Vector3(transform.parent.localScale.normalized.x < 0 ? 1 : -1, 1, 0)); // Object pos as player pos

            _mousePos.x -= objectPos.x;
            _mousePos.y -= objectPos.y;

            _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg - 90;
            // rounds to nearest 45 degree interval
            _angle = Mathf.RoundToInt(_angle / 45) * 45;
            // takes the top point off
            _angle = _angle > -45 && _angle <= 0 ? -45 : _angle < 45 && _angle >= 0 ? 45 : _angle;
            // takes the bottom point off
            _angle = _angle > -225 && _angle <= -180 ? -225 : _angle < -135 && _angle >= -180 ? -135 : _angle;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
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

        public void DisableFlashlight()
        {
            HasFlashlight = false;
            StopAllCoroutines();
        }
    }
}
