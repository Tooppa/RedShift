using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private ParticleSystem _gun;

        public float cooldown;
        public float counter;

        private void Start()
        {
            _gun = GetComponentInChildren<ParticleSystem>();
            counter = cooldown;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) && _gun.gameObject.activeSelf && counter > cooldown)
            {
                CameraEffects.Instance.ShakeCamera(1.5f, .1f);
                _gun.Play();
                counter = 0;
            }
            else
                _gun.Stop();

            counter += Time.deltaTime;
        }
    }
}
