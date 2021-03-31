using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private GameObject _gun;
        public bool HasGun  { private set; get; }

        private GameObject _audioController;
        private bool _cooldown = false;

        private void Start()
        {
            _gun = GameObject.Find("Gun");
            _audioController = GameObject.Find("AudioController");
            HasGun = true;
        }

        public void Shoot()
        {
            if (_cooldown || !HasGun || !_gun.gameObject.activeSelf) return;
            StartCoroutine(Cooldown(1));
            CameraEffects.Instance.ShakeCamera(1.5f, .1f);
            _gun.GetComponentInChildren<ParticleSystem>().Play();
            _audioController.GetComponent<SFX>().PlayGunShot();
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
        }
    }
}
