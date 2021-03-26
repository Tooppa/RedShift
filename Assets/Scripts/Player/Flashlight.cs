using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flashlight : MonoBehaviour
{
    private Light2D _light2D;
    private GameObject _gun;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _gun = GameObject.Find("Gun");
    }

    // Update is called once per frame
    private void Update()
    {
        // Enable and disable the flashlight
        if (Input.GetKeyDown(KeyCode.F))
        {
            _light2D.enabled = !_light2D.enabled;
            _gun.SetActive(!_gun.activeSelf);
        }

        var horizontalDirection = Input.GetAxisRaw("Horizontal");
        var verticalDirection = Input.GetAxisRaw("Vertical");

        // Flashlight left and right
        if (horizontalDirection != 0)
            transform.eulerAngles = new Vector3(0, 0, -90);
        {
            switch (horizontalDirection)
            {
                case 1:
                    transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case -1:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
            }
        }

            // Flashlight up and down
            if (verticalDirection != 0)
        {
            switch(verticalDirection)
            {
                case 1:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case -1:
                    transform.eulerAngles = new Vector3(0, 0, 180);
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