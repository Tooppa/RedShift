using UnityEngine;

/// <summary>
/// Creates a Shadow Caster 2D to the gameobject if a supported Collider 2D is found.
/// Supported colliders are PolygonCollider2D and CompositeCollider2D.
/// </summary>
public class AutomaticShadowCaster2D : MonoBehaviour
{
    private void Start()
    {
        // Find a supported collider type
        if (TryGetComponent(out PolygonCollider2D polygonCollider2D))
        {}
    }

}
