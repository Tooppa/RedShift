using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Glow : MonoBehaviour
{
    [SerializeField] private float changeSpeed;
    [SerializeField] private float maxIntensity;
    
    [Tooltip("These will replace whatever color you have set to the present Light2D. The colors will be shown in the order they are defined.")]
    [SerializeField] private Color[] colors;

    private Light2D _light2D;

    // Start is called before the first frame update
    private void Start()
    {
        if (colors.Length == 0)
        {
            Debug.LogWarning($"No colors set for {gameObject.name}");
            return;
        }
        
        _light2D = GetComponent<Light2D>();
        StartCoroutine(Cycle());
    }

    private void OnEnable()
    {
        if (_light2D != null)
            StartCoroutine(Cycle());
    }
    
    /// <summary>
    /// Glow every light in the order they are stored in the array.
    /// </summary>
    /// <remarks>
    /// Default intended functionality assumes that the light's intensity is at zero at start.
    /// If the Light2D had it's own color property set, it will be overridden by this script.
    /// </remarks>
    private IEnumerator Cycle()
    {
        foreach (var color in colors)
        {
            if (!enabled) yield break;
            
            _light2D.color = color;
            
            while (_light2D.intensity < maxIntensity)
            {
                _light2D.intensity += Time.deltaTime * changeSpeed; // Increase intensity
    
                yield return null;
            }
            while (_light2D.intensity > 0)
            {
                _light2D.intensity -= Time.deltaTime * changeSpeed; // Decrease intensity
    
                yield return null;
            }
        }

        StartCoroutine(Cycle());
    }
    
}