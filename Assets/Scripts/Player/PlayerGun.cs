using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private ParticleSystem gun;

        private void Start()
        {
            gun = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                gun.Play();
            else if (Input.GetKeyUp(KeyCode.R))
                gun.Stop();
        }
    }
}
