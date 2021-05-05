using System;
using System.Collections;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        public string startLocation;
        private int _fuel;
        private bool _pickableRange;

        private SFX _audioController;
        
        private GameObject _pickableItem;
        private CanvasManager _canvasManager;
        private PlayerMovement _playerMovement;
        private PlayerGun _playerGun;
        public PlayerControls playerControls;
        private Flashlight _flashlight;
        private Health _health;
        private ForceGlove _forceGlove;
        private bool _inCooldown = false;
        private string _currentLocation;

        private Light2D playerVitalSignLight;

        private Animator _animator;
        private static readonly int Death = Animator.StringToHash("Death");

        private const float TimeBeforeRespawn = 2f;
        private Action<InputAction.CallbackContext> _interactHandler;

        private void Awake()
        {
            _audioController = GameObject.Find("AudioController").GetComponent<SFX>();
            playerControls = new PlayerControls();
            _currentLocation = startLocation;
            _pickableItem = new GameObject();
        }

        private void Start()
        {
            _canvasManager = CanvasManager.Instance;
            _canvasManager.SetFuel(_fuel);
            _playerMovement = gameObject.GetComponent<PlayerMovement>();
            _playerGun = gameObject.GetComponentInChildren<PlayerGun>();
            _flashlight = gameObject.GetComponentInChildren<Flashlight>();
            _forceGlove = gameObject.GetComponentInChildren<ForceGlove>();
            PointEquipment(new Vector2(1, 0));
            
            playerControls.Surface.Jump.performed += ctx => _playerMovement.Jump(ctx.ReadValue<float>());
            playerControls.Surface.Dash.started += _ => _playerMovement.Dash();
            playerControls.Surface.OpenHud.started += _ => _canvasManager.SetHudActive();
            playerControls.Surface.PauseMenu.started += _ => _canvasManager.SetPauseMenuActive();
            playerControls.Surface.Shoot.performed += ctx => _playerGun.Shoot(ctx.ReadValue<float>());
            playerControls.Surface.Flashlight.started += _ => SwitchEquipment();
            playerControls.Surface.Push.started += _ => _forceGlove.Push();
            _interactHandler = _ => PickItem();
            playerControls.Surface.Interact.started += _interactHandler;
            
            _health = gameObject.GetComponent<Health>();
            _health.TakingDamage += OnTakingDamage;

            // Prefab guarantees existence
            playerVitalSignLight = GameObject.FindWithTag("PlayerVitalSignLight").GetComponent<Light2D>();

            if(SaveAndLoad.SaveLoadingWaitsInstructions)
                SaveAndLoad.FinishLoadingSave();

            _animator = transform.GetChild(1).GetComponent<Animator>();
        }

        private void Update()
        {
            if (Time.timeScale != 1) return;

            var move = playerControls.Surface.Move.ReadValue<Vector2>();
            
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
            _playerGun.gameObject.SetActive(!_playerGun.gameObject.activeInHierarchy);
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
            _playerGun.gameObject.transform.eulerAngles = new Vector3(0, 0, newAngle);
            _forceGlove.transform.eulerAngles = new Vector3(0, 0, newAngle);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            _canvasManager.ShowInteract(other.transform);
        }

        public InputAction ChangeInteract()
        {
            playerControls.Surface.Interact.started -= _interactHandler;
            return playerControls.Surface.Interact;
        }
        public void ReEnableInteract()
        {
            playerControls.Surface.Interact.started += _interactHandler;
        }

        private void SpecialPickups(GameObject go)
        {
            var pickables = go.GetComponent<Pickables>();
            
            SaveAndLoad.CurrentlyPickedItems.Add(pickables.data.name);
            
            if (pickables.HasFuel)
            {
                _fuel += pickables.fuel;
                pickables.fuel = 0;
                _canvasManager.SetFuel(_fuel);
                _audioController.PlayEnergyCapsulePickUp();
            }
            if (pickables.RocketBoots && !_playerMovement.HasRocketBoots)
            {
                _playerMovement.EquipRocketBoots();
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
                _audioController.PlayPowerUp();
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
                _audioController.PlayPowerUp();
            }
            if (pickables.ForceGlove && !_forceGlove.HasGlove)
            {
                _forceGlove.EquipGlove();
                _canvasManager.AddNewUpgrade(pickables.GetSprite(), pickables.GetStats());
                _audioController.PlayPowerUp();
            }
            if (pickables.Flashlight && !_flashlight.HasFlashlight)
            {
                _flashlight.EquipFlashlight();
                _flashlight.SwitchLight();
                _playerGun.gameObject.SetActive(!_playerGun.gameObject.activeInHierarchy);
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

        /// <summary>
        /// Picks an item if found on range. If <see cref="constructedObject"/> is valid, that will be picked instead
        /// and usually considered <see cref="_pickableRange"/> will be ignored.
        /// </summary>
        /// <param name="constructedObject">If valid, <see cref="_pickableRange"/> will be ignored and this will be set as
        /// <see cref="_pickableItem"/> </param>
        public void PickItem(GameObject constructedObject = null)
        {
            if (constructedObject == null)
            {
                if (!_pickableRange) return; 
            }
            else
            {
                _pickableItem = constructedObject;
            }
            
            SpecialPickups(_pickableItem);
            
            _pickableItem.gameObject.SetActive(false);
            
            if (!_pickableItem.TryGetComponent(out Pickables component) || !component.IsNote) return;
            
            // Don't open the note when loading a save
            if(constructedObject == null)
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
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
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

            if (healthPercent <= 0)
            {
                StartCoroutine(PlayerDie());
            }
        }
        
        private IEnumerator PlayerDie()
        {
            DisableMovement();
            
            _animator.SetTrigger(Death);
            
            // Knock player back a little. Get direction from the players facing direction
            GetComponent<Rigidbody2D>().AddForce(new Vector2((transform.localScale.x), 0f) * 15, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(TimeBeforeRespawn);
            
            SaveAndLoad.StartLoadingSave();
        }
        
        public void DisableMovement()
        {
            playerControls.Disable();
        }
        public void EnableMovement()
        {
            playerControls.Enable();
        }

        public int GetFuel()
        {
            return _fuel;
        }
    }
}
