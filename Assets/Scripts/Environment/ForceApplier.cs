using UnityEngine;

/// <summary>
/// Add force(s) to the assigned RigidBody2D. Great for events. Simple configuration and simple functionality.
/// </summary>
/// <example>
/// See <see cref="Trigger"/> which can be used to launch events like these.
/// Set up all 3 parameters of the ForceApplier and then set <see cref="Affect"/> to be the launched event.
/// </example>
/// <example>
/// Another way is to use this script straight trough code. All parameters are public and ready to be set. Use <see cref="Affect"/> to begin affecting.
/// </example>
public class ForceApplier : MonoBehaviour
{
    /// <summary>
    /// This the <see cref="Rigidbody2D"/> that will be affected. If it's not found, the script destroys itself at <see cref="Start"/>.
    /// </summary>
    public Rigidbody2D affectedRigidBody2D;
    
    /// <summary>
    /// Describes the direction and the power in which the the <see cref="affectedRigidBody2D"/> will be affected.
    /// </summary>
    public Vector2 affectVector;
    
    /// <summary>
    /// The force mode. For simple one-time events <see cref="ForceMode2D.Impulse"/> is probably the best option.
    /// If, however, the force is desired to be continuous, use <see cref="ForceMode2D.Force"/>
    /// </summary>
    public ForceMode2D forceMode2D;
    
    // Start is called before the first frame update
    private void Start()
    {
        if(affectedRigidBody2D == null)
        {
            Debug.LogWarning($"No rigidbody found in {gameObject.name}");
            Destroy(this);
        }
    }

    /// <summary>
    /// Calls <see cref="Rigidbody2D.AddForce"/> with the parameters
    /// <see cref="affectVector"/> and <see cref="forceMode2D"/>
    /// </summary>
    // TODO: This is not exactly null safe
    public void Affect() => affectedRigidBody2D.AddForce(affectVector, forceMode2D);
}
