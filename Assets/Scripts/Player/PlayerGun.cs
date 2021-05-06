using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        public float powerShotTimer;
        public bool HasGun  { private set; get; }
        public bool HasPowerfulGun  { private set; get; }
        
        private bool _equipped;
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

        public Transform _player;
        private PlayerControls _playerControls;
        private PlayerMechanics _playerMechanics;

        private SFX _sfx;
        private Camera _main;
        private float _angle;
        private Vector2 _mousePos;

        private void Awake()
        {
            _light2D = GetComponentInChildren<Light2D>();
        }

        private void Start()
        {
            _main = Camera.main;
            _audioController = GameObject.Find("AudioController");
            _sfx = _audioController.GetComponent<SFX>();

            // Unity is so bonkers that it returns the parent's component with this statement
            // Skip the parent's animator. Player's prefab guarantees the Sprite-object to have an animator
            _animator = transform.parent.GetComponentsInChildren<Animator>()[1];
            
            _intensity = _light2D.intensity;
            _light2D.intensity = 0;

            _weakShotEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
            _powerShotEffect = transform.GetChild(2).GetComponent<ParticleSystem>();
            _chargeEffect = transform.GetChild(3).GetComponent<ParticleSystem>();
            _chargeReadyEffect = transform.GetChild(4).GetComponent<ParticleSystem>();

            _player = transform.parent.GetComponent<Transform>();
            _playerMechanics = transform.GetComponentInParent<PlayerMechanics>();
            _playerControls = _playerMechanics.playerControls;
        }

        public void SwitchGun()
        {
            _equipped = !_equipped;
            _light2D.enabled = !_light2D.enabled;
            _holdingShoot = false;
            _playerControls.Surface.Move.Enable();
            _playerControls.Surface.Jump.Enable();
        }

        public void Shoot(float value)
        {
            if (Time.timeScale != 1 || !HasGun || !_equipped) return;
            _player.localScale = new Vector3(_mousePos.normalized.x > 0 ? 1 : -1, 1, 1); // Flip player to face towards the shooting direction
            _holdingShoot = value > 0;
            if (_cooldown)
            {
                _playerControls.Surface.Move.Enable();
                _playerControls.Surface.Jump.Enable();
                return;
            }
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
            particleCollision.weakShot = false;
            _animator.SetTrigger(ShootTrigger);
            StartCoroutine(Cooldown(1));
            CameraEffects.Instance.ShakeCamera(1.5f, .1f);
            _powerShotEffect.Play();
            _sfx.playerPowerfulCharge.Stop();
            _sfx.playerPowerfulShotChargedUp.Stop();
            _sfx.PlayPowerfulShot();

            _holdingShoot = false;
            _playerControls.Surface.Move.Enable();
            _playerControls.Surface.Jump.Enable();
        }

        private void WeakShot(ParticleCollision particleCollision)
        {
            particleCollision.weakShot = true;
            _animator.SetTrigger(ShootTrigger);
            StartCoroutine(Cooldown(1));
            CameraEffects.Instance.ShakeCamera(.5f, .1f);
            _weakShotEffect.Play();
            _sfx.playerPowerfulCharge.Stop();
            _sfx.PlayGunShot();

            _holdingShoot = false;
            _playerControls.Surface.Move.Enable();
            _playerControls.Surface.Jump.Enable();
        }

        private void Update()
        {
            if (_holdingShoot)
            {
                _playerControls.Surface.Move.Disable();
                _playerControls.Surface.Jump.Disable();
                
                if (HasPowerfulGun && _chargeTimer >= powerShotTimer)
                {
                    _chargeEffect.Stop();
                    _chargeReadyEffect.Play();
                    if(!_sfx.playerPowerfulShotChargedUp.isPlaying)
                        _sfx.PlayPowerfulShotChargedUp();
                }
                if (HasPowerfulGun && !chargeSFXPlaying)
                {
                    _animator.SetBool("GunCharging", true);
                    _chargeEffect.Play();
                    _sfx.PlayPowerfulCharge();
                    chargeSFXPlaying = true;
                }

                _chargeTimer += Time.deltaTime;
            }
            else
            {
                _animator.SetBool("GunCharging", false);
                _chargeReadyEffect.Stop();
                _chargeEffect.Stop();
                _sfx.playerPowerfulCharge.Stop();
                chargeSFXPlaying = false;
                _chargeTimer = 0;
            }
            PointGun();
        }

        private void PointGun()
        {
            if (!HasGun) return;
            _mousePos = Mouse.current.position.ReadValue();
            var objectPos = _main.WorldToScreenPoint(_player.position + Vector3.up*1.5f);
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
            _equipped = true;
            _light2D.enabled = true;
            _light2D.intensity = _intensity;
        }
        public void EquipPowerfulGun()
        {
            HasPowerfulGun = true;
        }
    }
}
