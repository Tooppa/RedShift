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
            if (Input.GetKeyDown(KeyCode.R) && _gun.gameObject.activeSelf)
            {
                CameraEffects.Instance.ShakeCamera(1.5f, .1f);
                _gun.Play();
            }
            else if (Input.GetKeyUp(KeyCode.R))
                _gun.Stop();
        }
    }
}
