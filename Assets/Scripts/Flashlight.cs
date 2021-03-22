using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flashlight : MonoBehaviour
{
    private Light2D _light2D;
    
    private void Start() => _light2D = GetComponent<Light2D>();

    // Update is called once per frame
    private void Update()
    {
        // Enable and disable the flashlight
        if (Input.GetKeyDown(KeyCode.F))
            _light2D.enabled = !_light2D.enabled;
        
        // Rotate the flashlight around the user's mouse
        
        var transformCache = transform;
        
        var flashlightToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transformCache.position;
        flashlightToMouse.z = 0; // Negate z axis that came from the camera
        
        var angleFromFlashLightToMouse = Vector3.SignedAngle(transformCache.up, flashlightToMouse, Vector3.forward);
        
        // Rotate the flashlight by the specific angle
        transform.Rotate(Vector3.forward, angleFromFlashLightToMouse);
        
    }
}
