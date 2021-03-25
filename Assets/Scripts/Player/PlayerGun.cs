using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        //public Transform firePoint;
        //public GameObject bullet;
        //private GameObject _flashlight;
        //private Vector2 _shootDirection;

        private ParticleSystem gun;

        private void Start()
        {
            gun = GetComponentInChildren<ParticleSystem>();
            //_shootDirection = Vector2.right;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                gun.Play();
            else if (Input.GetKeyUp(KeyCode.R))
                gun.Stop();

            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    //var x = _shootDirection.x;
            //    //var y = _shootDirection.y;
            //    //_shootDirection = new Vector2(y, -x);
            //}
        }

        //void Shoot()
        //{
        //    Instantiate(bullet, firePoint.position, firePoint.rotation);
        //}
    }
}
