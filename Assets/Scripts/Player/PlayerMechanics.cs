using System;
using UnityEngine;

namespace Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        private int _fuel = 0;
        private int _health = 10;
        private bool _pickableRange = false;
        private CanvasManager _canvasManager;
        private PlayerMovement _playerMovement;
        private PlayerGun _playerGun;
        private PlayerControls _playerControls;
        private Flashlight _flashlight;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void Start()
        {
            _canvasManager = CanvasManager.Instance;
            _canvasManager.SetFuel(_fuel);
            _canvasManager.SetHealth(_health);
            _playerMovement = gameObject.GetComponent<PlayerMovement>();
            _playerGun = gameObject.GetComponent<PlayerGun>();
            _flashlight = gameObject.GetComponentInChildren<Flashlight>();
            
            _playerControls.Surface.Jump.started += _ => _playerMovement.Jump();
            _playerControls.Surface.Dash.started += _ => _playerMovement.Dash();
            _playerControls.Surface.OpenHud.started += _ => _canvasManager.SetHudActive();
            _playerControls.Surface.Shoot.started += _ => _playerGun.Shoot();
            _playerControls.Surface.Flashlight.started += _ => _flashlight.SwitchLight();
        }

        private void Update()
        {
            var move = _playerControls.Surface.Move.ReadValue<Vector2>();
            if (Time.timeScale != 1) return;
            _playerMovement.Movement(move);
            if(move.x != 0 || move.y != 0)
                _flashlight.PointFlash(move);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;

            var go = other.gameObject;
            var pickables = go.GetComponent<Pickables>();
            var sprite = go.GetComponent<SpriteRenderer>().sprite;
            
            if (pickables.IsNote)
            {
                pickables.ShowInteract();
                return;
            }
            SpecialPickups(pickables, sprite);
            go.SetActive(false);
            _canvasManager.AddNewImage(sprite);
        }

        private void SpecialPickups(Pickables pickables, Sprite sprite)
        {
            if (pickables.HasFuel)
            {
                _fuel += pickables.fuel;
                pickables.fuel = 0;
                _canvasManager.SetFuel(_fuel);
            }
            if (pickables.RocketBoots && !_playerMovement.HasRocketBoots)
            {
                _playerMovement.EquipRocketBoots();
                _canvasManager.AddNewUpgrade(sprite, pickables.GetStats());
            }
            if (pickables.Gun && !_playerGun.HasGun)
            {
                _playerGun.EquipGun();
                _canvasManager.AddNewUpgrade(sprite, pickables.GetStats());
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            var go = other.gameObject;
            var pickables = go.GetComponent<Pickables>();
            if (!pickables.IsNote || !Input.GetKey(KeyCode.E)) return;
            var sprite = go.GetComponent<SpriteRenderer>().sprite;
            _canvasManager.ShowText(pickables.getNote());
            SpecialPickups(pickables, sprite);
            _canvasManager.AddNewNote(go);
            other.gameObject.SetActive(false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            var pickables = other.gameObject.GetComponent<Pickables>();
            if (pickables.IsNote)
                pickables.HideInteract();
        }
        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }
    }
}
