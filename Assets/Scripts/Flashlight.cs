using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        var transformCache = transform;
        
        var flashlightToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transformCache.position;
        flashlightToMouse.z = 0; // Negate z axis that came from the camera
        
        var angleFromFlashLightToMouse = Vector3.SignedAngle(transformCache.up, flashlightToMouse, Vector3.forward);
        
        // Rotate the flashlight by the angle
        transform.Rotate(Vector3.forward, angleFromFlashLightToMouse);
        
    }
}
