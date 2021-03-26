using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private ParticleSystem _gun;

        private void Start()
        {
            _gun = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                _gun.Play();
            else if (Input.GetKeyUp(KeyCode.R))
                _gun.Stop();
        }
    }
}
