using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Tooltip("Check this if this script should be responsible of destroying the original object after breaking.")]
    [SerializeField] private bool destroyOriginalObject;
    
    private readonly List<Transform> _pieces = new List<Transform>(); // All children that have a PolygonCollider
    
    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            
            // Only add the child if it has a polygon collider and a sprite mask
            if (child.TryGetComponent(out PolygonCollider2D _) && child.TryGetComponent(out SpriteRenderer _)) 
                _pieces.Add(child);
        }
    }
    
    /// <summary>
    /// Breaks the object to pieces that are pre-defined as the children of the object.
    /// The children will be detached from the parent so the original object can be safely destoyed.
    /// </summary>
    public void BreakApart()
    {
        transform.DetachChildren(); // The original object can be safely removed

        foreach (var piece in _pieces)
        {
            // Enable the collider, rigidbody and the sprite renderer of the new broken piece
            
            var polygonCollider2D = piece.GetComponent<PolygonCollider2D>();
            polygonCollider2D.enabled = true;

            piece.GetComponent<Rigidbody2D>().isKinematic = false;

            piece.GetComponent<SpriteRenderer>().enabled = true;
            
            piece.gameObject.AddComponent<AutomaticShadowCaster2D>();
        }
        
        if(destroyOriginalObject)
            Destroy(gameObject);
    }
}
