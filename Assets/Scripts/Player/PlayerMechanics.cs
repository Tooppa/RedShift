using UnityEngine;

namespace Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        private int _fuel;
        private int _health = 10;
        private bool _pickableRange;
        
        private GameObject _pickableNote;
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
            PointEquipment(new Vector2(1, 0));
            
            _playerControls.Surface.Jump.started += _ => _playerMovement.Jump();
            _playerControls.Surface.Dash.started += _ => _playerMovement.Dash();
            _playerControls.Surface.OpenHud.started += _ => _canvasManager.SetHudActive();
            _playerControls.Surface.Shoot.started += _ => _playerGun.Shoot();
            _playerControls.Surface.Flashlight.started += _ => SwitchEquipment();
            _playerControls.Surface.Interact.started += _ => ReadNote();
        }

        private void Update()
        {
            if (Time.timeScale != 1) return;

            var move = _playerControls.Surface.Move.ReadValue<Vector2>();
            
            _playerMovement.Movement(move);
            
            if(move.x != 0 || move.y != 0)
               PointEquipment(move);
        }

        // Switch between:
        // Stronger flashlight and no gun
        // Weaker flashlight and gun
        private void SwitchEquipment()
        {
            _flashlight.SwitchLight();
            _playerGun.gun.SetActive(!_playerGun.gun.activeInHierarchy);
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

            var go = other.gameObject;
            var pickables = go.GetComponent<Pickables>();
            var sprite = go.GetComponent<SpriteRenderer>().sprite;
            
            if (pickables.IsNote || pickables.Flashlight)
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
            if (!other.gameObject.GetComponent<Pickables>().IsNote) return;
            _pickableRange = true;
            _pickableNote = other.gameObject;
        }

        public void ReadNote()
        {
            var note = _pickableNote.GetComponent<Pickables>();
            if (!_pickableRange || !note.IsNote) return;
            
            var sprite = _pickableNote.GetComponent<SpriteRenderer>().sprite;
            _canvasManager.ShowText(note.getNote());
            SpecialPickups(note, sprite);
            _canvasManager.AddNewNote(_pickableNote);
            _pickableNote.gameObject.SetActive(false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;
            var pickables = other.gameObject.GetComponent<Pickables>();
            if (!pickables.IsNote) return;
            pickables.HideInteract();
            _pickableRange = false;
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
