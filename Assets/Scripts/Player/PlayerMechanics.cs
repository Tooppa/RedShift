using System;
using System.Collections;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        public string startLocation;
        private int _fuel;
        private bool _pickableRange;
        
        private GameObject _pickableItem;
        private CanvasManager _canvasManager;
        private PlayerMovement _playerMovement;
        private PlayerGun _playerGun;
        private PlayerControls _playerControls;
        private Flashlight _flashlight;
        private Health _health;
        private bool _inCooldown = false;
        private string _currentLocation;

        private Light2D playerVitalSignLight;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _currentLocation = startLocation;
            _pickableItem = new GameObject();
        }

        private void Start()
        {
            _canvasManager = CanvasManager.Instance;
            _canvasManager.SetFuel(_fuel);
            _playerMovement = gameObject.GetComponent<PlayerMovement>();
            _playerGun = gameObject.GetComponent<PlayerGun>();
            _flashlight = gameObject.GetComponentInChildren<Flashlight>();
            PointEquipment(new Vector2(1, 0));
            
            _playerControls.Surface.Jump.performed += ctx => _playerMovement.Jump(ctx.ReadValue<float>());
            _playerControls.Surface.Dash.started += _ => _playerMovement.Dash();
            _playerControls.Surface.OpenHud.started += _ => _canvasManager.SetHudActive();
            _playerControls.Surface.Shoot.performed += ctx => _playerGun.Shoot(ctx.ReadValue<float>());
            _playerControls.Surface.Flashlight.started += _ => SwitchEquipment();
            _playerControls.Surface.Interact.started += _ => PickItem();
            
            _health = gameObject.GetComponent<Health>();
            _health.TakingDamage += OnTakingDamage;

            // Prefab guarantees existence
            playerVitalSignLight = GameObject.FindWithTag("PlayerVitalSignLight").GetComponent<Light2D>();
        }

        private void Update()
        {
            if (Time.timeScale != 1) return;

            var move = _playerControls.Surface.Move.ReadValue<Vector2>();
            
            _playerMovement.Movement(move);
            
            if(move.x != 0 || move.y != 0)
               PointEquipment(move);
        }
        
        private IEnumerator Cooldown()
        {
            //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
            _inCooldown = true;
            yield return new WaitForSeconds(.3f);
            _inCooldown = false;
        }

        // Switch between:
        // Stronger flashlight and no gun
        // Weaker flashlight and gun
        private void SwitchEquipment()
        {
            if (!_flashlight.HasFlashlight || !_playerGun.HasGun || _inCooldown) return;
            _flashlight.SwitchLight();
            _playerGun.gun.SetActive(!_playerGun.gun.activeInHierarchy);
            StartCoroutine(Cooldown());
        }
        
        private void PointEquipment(Vector2 move)
        {
            var horizontalMove = move.x;
            var verticalMove = move.y;
            
            // if horizontal and vertical are both pressed or vertical is below 0
            // and if vertical is below 0 down else up
            // else if horizontal is pressed left or right
            var newAngle = horizontalMove != 0 && verticalMove != 0 || verticalMove < 0 ? verticalMove < 0 ? 180 :
                0 :
                horizontalMove != 0 ? -90 * horizontalMove : 0;
            
            _flashlight.transform.eulerAngles = new Vector3(0, 0, newAngle);
            _playerGun.gun.transform.eulerAngles = new Vector3(0, 0, newAngle);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            _canvasManager.ShowInteract(other.transform);
        }

        private void SpecialPickups(GameObject go)
        {
            var pickables = go.GetComponent<Pickables>();
            if (pickables.HasFuel)
            {
                _fuel += pickables.fuel;
                pickables.fuel = 0;
                _canvasManager.SetFuel(_fuel);
            }
            if (pickables.RocketBoots && !_playerMovement.HasRocketBoots)
            {
                _playerMovement.EquipRocketBoots();
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
            }
            if (pickables.Gun && !_playerGun.HasGun)
            {
                _playerGun.EquipGun();
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
            }
            if (pickables.PowerfulGun && !_playerGun.HasPowerfulGun)
            {
                _playerGun.EquipPowerfulGun();
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
            }
            if (pickables.Flashlight && !_flashlight.HasFlashlight)
            {
                _flashlight.EquipFlashlight();
                _flashlight.SwitchLight();
                _playerGun.gun.SetActive(!_playerGun.gun.activeInHierarchy);
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
            }
            if (go.TryGetComponent(out Trigger trigger))
                trigger.@event.Invoke();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            _pickableRange = true;
            _pickableItem = other.gameObject;
        }

        private void PickItem()
        {
            if (!_pickableRange) return;
            SpecialPickups(_pickableItem);
            _pickableItem.gameObject.SetActive(false);
            if (!_pickableItem.TryGetComponent(out Pickables component) || !component.IsNote) return;
            _canvasManager.ShowText(component.GetNote(), component.GetPicture());
            var sprite = _pickableItem.GetComponent<SpriteRenderer>().sprite;
            _canvasManager.AddNewNote(sprite, component.GetPicture(), component.GetNote(), _currentLocation);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            _canvasManager.HideInteract();
            _pickableRange = false;
        }

        public void ChangeLocation(string newLocation)
        {
            _currentLocation = newLocation;
            //Special case for rocket
            if (newLocation == "Rocket")
            {
                _canvasManager.ShowRocketButton();
            }
        }

        public void DisablePlayerLightsForSeconds(float sec)
        {
            StartCoroutine(DisableLights(sec));
        }

        private IEnumerator DisableLights(float sec)
        {
            var light2Ds = GetComponentsInChildren<Light2D>();
            Flashlight flashlight = null;
            var values = new List<float>();
            foreach (var t in light2Ds)
            {
                if(t.TryGetComponent(out flashlight))
                    flashlight.DisableFlashlight();
                values.Add(t.intensity);
                t.intensity = 0;
            }
            yield return new WaitForSeconds(sec);
            for (var i = 0; i < light2Ds.Length; i++)
            {
                light2Ds[i].intensity = values[i];
            }
            if (flashlight)
                flashlight.EquipFlashlight();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        /// <summary>
        /// Switches the health indicator led's color based on current percentage of health.
        /// If health drops to zero or below, the player dies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnTakingDamage(object sender, EventArgs eventArgs)
        {
            var healthPercent = _health.CurrentHealth / _health.MaxHealth;
            
            var ledColor = healthPercent switch
            {
                var n when (n > 0.66) => Color.green,
                var n when (n > 0.33) => Color.yellow,
                _ => Color.red
            };

            playerVitalSignLight.color = ledColor;
        }

        public void DisableMovement()
        {
            _playerControls.Disable();
        }
        public void EnableMovement()
        {
            _playerControls.Enable();
        }
    }
}
