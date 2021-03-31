using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    [RequireComponent(typeof(Light2D))]
    public class Flashlight : MonoBehaviour
    {
        private Light2D _light2D;
        private GameObject _gun;
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
            _gun = GameObject.Find("Gun");
            _audioController = GameObject.Find("AudioController");
            _flickerTime = 2;
            _intensity = _light2D.intensity;
            PointFlash(new Vector2(1, 0));
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_light2D.enabled) return;

            if (_flickerTimer >= _flickerTime)
                StartCoroutine(Flicker());
            _flickerTimer += Time.deltaTime;

            // FlashLight rotation temporarily (or permanently) disabled
            // Rotate the flashlight around the user's mouse
            /*
        var transformCache = transform;
        
        var flashlightToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transformCache.position;
        flashlightToMouse.z = 0; // Negate z axis that came from the camera
        
        var angleFromFlashLightToMouse = Vector3.SignedAngle(transformCache.up, flashlightToMouse, Vector3.forward);
        
        // Rotate the flashlight by the specific angle
        transform.Rotate(Vector3.forward, angleFromFlashLightToMouse);
        */
        }

        public void PointFlash(Vector2 move)
        {
            var horizontal = move.x;
            var vertical = move.y;
            // if horizontal and vertical are both pressed or vertical is below 0
            // and if vertical is below 0 down else up
            // else if horizontal is pressed left or right
            var newAngle = horizontal != 0 && vertical != 0 || vertical < 0 ? vertical < 0 ? 180 :
                0 :
                horizontal != 0 ? -90 * horizontal : 0;
            transform.eulerAngles = new Vector3(0, 0, newAngle);
            _gun.transform.eulerAngles = new Vector3(0, 0, newAngle);
        }
        public void SwitchLight()
        {
            if (_inCooldown) return;
            StartCoroutine(Cooldown());
            _audioController.GetComponent<SFX>().PlayClick();
            _light2D.enabled = !_light2D.enabled;
            _light2D.intensity = _intensity;
            _gun.SetActive(!_gun.activeSelf);

            StartCoroutine(Cooldown());
        }
    }
}
