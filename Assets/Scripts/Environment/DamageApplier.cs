using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies damage by calculating the momentum of the object between collisions. Momentum is speed * mass.
/// The damage is only applied to objects specified in <see cref="tags"/> and from those only the ones that have an active
/// <see cref="Health"/> script.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class DamageApplier : MonoBehaviour
{
    /// <summary>
    /// Only apply damage to <see cref="GameObject"/>'s carrying one of these tags.
    /// </summary>
    [Tooltip("Which tags should the collision apply damage to.")]
    [SerializeField] private List<string> tags = new List<string>();
    
    /// <summary>
    /// The resulting momentum can be further multiplied which gives flexibility in defining the mass of the object.
    /// </summary>
    [Tooltip("Multiplies the momentum by specified amount.")]
    [SerializeField] private float damageMultiplier;

    /// <summary>
    /// Can be tweaked to further configure the occasion's when damage should apply. This prevents seemingly still objects
    /// from applying damage.
    /// </summary>
    [Tooltip("If the object's speed is below the treshold, no damage will be applied. Recommended value is >= 1.5")]
    [SerializeField] private float minimumSpeedTreshold;

    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    private void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!tags.Contains(other.gameObject.tag) || !other.gameObject.TryGetComponent(out Health otherObjectHealth)) 
            return;

        var speedAbs = Mathf.Abs(_rigidbody2D.velocity.magnitude);
        if (speedAbs < minimumSpeedTreshold) return;
        
        var momentum = speedAbs * _rigidbody2D.mass * damageMultiplier;
        
        otherObjectHealth.TakeDamage(momentum);
    }
}
