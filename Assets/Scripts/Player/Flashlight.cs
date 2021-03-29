using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flashlight : MonoBehaviour
{
    private Light2D _light2D;
    private GameObject _gun;
    private GameObject _audioController;

    //public float cooldownTime;
    //private bool inCooldown;

    //private IEnumerator Cooldown()
    //{
    //    //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
    //    inCooldown = true;
    //    yield return new WaitForSeconds(cooldownTime);
    //    inCooldown = false;
    //}

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _gun = GameObject.Find("Gun");
        _audioController = GameObject.Find("AudioController");
    }

    // Update is called once per frame
    private void Update()
    {
        // Enable and disable the flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            _light2D.enabled = !_light2D.enabled;
            _gun.SetActive(!_gun.activeSelf);
            _audioController.GetComponent<SFX>().PlayClick();

            //StartCoroutine(Cooldown());
        }

        var horizontalDirection = Input.GetAxisRaw("Horizontal");
        var verticalDirection = Input.GetAxisRaw("Vertical");

        // Flashlight up and down
        if (verticalDirection != 0)
        {
            switch (verticalDirection)
            {
                case 1:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    _gun.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case -1:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    _gun.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
            }
        }

        // Flashlight left and right
        else if (horizontalDirection != 0)
        {
            switch (horizontalDirection)
            {
                case 1:
                    transform.eulerAngles = new Vector3(0, 0, -90);
                    _gun.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case -1:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    _gun.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
            }
        }

        // FlashLight rotation temporarily (or permanently) disabled
        // Rotate the flashlight around the user's mouse
        /*
        var transformCache = transform;
        
        var flashlightToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transformCache.position;
        flashlightToMouse.z = 0; // Negate z axis that came from the camera
        
        var angleFromFlashLightToMouse = Vector3.SignedAngle(transformCache.up, flashlightToMouse, Vector3.forward);
        
        // Rotate the flashlight by the specific angle
        transform.Rotate(Vector3.forward, angleFromFlashLightToMouse);
        */
    }
}
