using UnityEngine;

namespace Player
{
    public class PlayerMechanics : MonoBehaviour
    {
        private int _fuel = 0;
        private int _health = 10;
        private CanvasManager _canvasManager;

        private void Start()
        {
            _canvasManager = CanvasManager.Instance;
            _canvasManager.SetFuel(_fuel);
            _canvasManager.SetHealth(_health);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Pickable")) return;

            var go = other.gameObject;
            var pickables = go.GetComponent<Pickables>();

            if (pickables.HasFuel)
            {
                _fuel += pickables.fuel;
                pickables.fuel = 0;
                _canvasManager.SetFuel(_fuel);
            }
            if (pickables.RocketBoots)
            {
                gameObject.GetComponent<PlayerMovement>().EquipRocketBoots();
            }
            if (pickables.IsNote)
            {
                pickables.ShowInteract();
                return;
            }
            go.SetActive(false);
            _canvasManager.AddNewImage(go.GetComponent<SpriteRenderer>().sprite);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var go = other.gameObject;
            var pickables = go.GetComponent<Pickables>();
            if (!pickables.IsNote || !Input.GetKey(KeyCode.E)) return;
            _canvasManager.ShowText(pickables.getNote());
            _canvasManager.AddNewNote(go);
            other.gameObject.SetActive(false);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var pickables = other.gameObject.GetComponent<Pickables>();
            if (pickables.IsNote)
                pickables.HideInteract();
        }
    }
}
