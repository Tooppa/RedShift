using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        public float powerShotTimer;
        public bool HasGun  { private set; get; }
        public bool HasPowerfulGun  { private set; get; }

        private GameObject _audioController;
        private bool _cooldown;
        private Light2D _light2D;
        private float _intensity;
        private float _chargeTimer = 0;
        private bool _holdingShoot;
        private bool chargeSFXPlaying = false;

        private Animator _animator;
        private static readonly int ShootTrigger = Animator.StringToHash("Shoot");

        private ParticleSystem _weakShotEffect;
        private ParticleSystem _powerShotEffect;
        private ParticleSystem _chargeEffect;
        private ParticleSystem _chargeReadyEffect;

        private void Start()
        {
            _audioController = GameObject.Find("AudioController");

            // Unity is so bonkers that it returns the parent's component with this statement
            // Skip the parent's animator. Player's prefab guarantees the Sprite-object to have an animator
            _animator = transform.parent.GetComponentsInChildren<Animator>()[1];
            
            _light2D = GetComponentInChildren<Light2D>();
            _intensity = _light2D.intensity;
            _light2D.intensity = 0;

            _weakShotEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
            _powerShotEffect = transform.GetChild(2).GetComponent<ParticleSystem>();
            _chargeEffect = transform.GetChild(3).GetComponent<ParticleSystem>();
            _chargeReadyEffect = transform.GetChild(4).GetComponent<ParticleSystem>();
        }

        public void Shoot(float value)
        {
            _holdingShoot = value > 0;
            if (_cooldown || !HasGun || !gameObject.activeSelf) return;
            var particleCollision = GetComponentInChildren<ParticleCollision>();
            if (HasPowerfulGun)
            {
                if (_holdingShoot) return;
                if (_chargeTimer >= powerShotTimer)
                    PowerfulShot(particleCollision);
                else WeakShot(particleCollision);
            }
            else if (_holdingShoot)
                WeakShot(particleCollision);
        }

        private void PowerfulShot(ParticleCollision particleCollision)
        {
            _animator.SetTrigger(ShootTrigger);
            StartCoroutine(Cooldown(1));
            particleCollision.DisableWeakShot();
            CameraEffects.Instance.ShakeCamera(1.5f, .1f);
            _powerShotEffect.Play();
            _audioController.GetComponent<SFX>().playerPowerfulCharge.Stop();
            _audioController.GetComponent<SFX>().PlayPowerfulShot();
        }

        private void WeakShot(ParticleCollision particleCollision)
        {
            _animator.SetTrigger(ShootTrigger);
            StartCoroutine(Cooldown(1));
            particleCollision.EnableWeakShot();
            CameraEffects.Instance.ShakeCamera(.5f, .1f);
            _weakShotEffect.Play();
            _audioController.GetComponent<SFX>().playerPowerfulCharge.Stop();
            _audioController.GetComponent<SFX>().PlayGunShot();
        }

        private void FixedUpdate()
        {
            if (_holdingShoot)
            {
                if (HasPowerfulGun && _chargeTimer >= powerShotTimer)
                {
                    _chargeEffect.Stop();
                    _chargeReadyEffect.Play();
                    _audioController.GetComponent<SFX>().PlayPowerfulShotChargedUp();
                }
                if (HasPowerfulGun && !chargeSFXPlaying)
                {
                    _animator.SetBool("GunCharging", true);
                    _chargeEffect.Play();
                    _audioController.GetComponent<SFX>().PlayPowerfulCharge();
                    chargeSFXPlaying = true;
                }

                _chargeTimer += Time.deltaTime;
            }
            else
            {
                _animator.SetBool("GunCharging", false);
                _chargeReadyEffect.Stop();
                _chargeEffect.Stop();
                _audioController.GetComponent<SFX>().playerPowerfulCharge.Stop();
                chargeSFXPlaying = false;
                _chargeTimer = 0;
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
