using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        public GameObject gun;
        public float powerShotTimer;
        public bool HasGun  { private set; get; }
        public bool HasPowerfulGun  { private set; get; }

        private GameObject _audioController;
        private bool _cooldown;
        private Light2D _light2D;
        private float _intensity;
        private float _chargeTimer = 0;
        private bool _holdingShoot;

        private void Start()
        {
            gun = GameObject.Find("Gun");
            _audioController = GameObject.Find("AudioController");
            _light2D = GetComponentInChildren<Light2D>();
            _intensity = _light2D.intensity;
            _light2D.intensity = 0;
        }

        public void Shoot(float value)
        {
            if (_cooldown || !HasGun || !gun.gameObject.activeSelf) return;
            if (HasPowerfulGun)
            {
                _holdingShoot = value > 0;
                if (_holdingShoot || _chargeTimer < powerShotTimer) return;

                StartCoroutine(Cooldown(1));
                CameraEffects.Instance.ShakeCamera(1.5f, .1f);
                gun.GetComponentInChildren<ParticleSystem>().Play();
                _audioController.GetComponent<SFX>().PlayGunShot();
            }
            else
            {
                StartCoroutine(Cooldown(1));
                //CameraEffects.Instance.ShakeCamera(1.5f, .1f);
                gun.GetComponentInChildren<ParticleSystem>().Play();
                _audioController.GetComponent<SFX>().PlayButtonBuzz();
            }
        }

        private void FixedUpdate()
        {
            if (_holdingShoot)
            {
                _chargeTimer += Time.deltaTime;
            }
            else
            {
                _chargeTimer =   0;
            }
        }
        private IEnumerator Cooldown(float cooldownTime)
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            _cooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _cooldown = false;
        }

        public void EquipGun()
        {
            HasGun = true;
            _light2D.intensity = _intensity;
        }
        public void EquipPowerfulGun()
        {
            HasPowerfulGun = true;
        }
    }
}
