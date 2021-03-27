using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class Breakable : MonoBehaviour
{
    [Tooltip("Check this if this script should be responsible of destroying the original object after breaking.")]
    [SerializeField] private bool destroyOriginalObject;
    
    [Tooltip("Pieces' Shadow casters are generated on the fly. Check this if self shadows should be included along with the default options.")]
    [SerializeField] private bool generateSelfShadowsForPieces;

    private readonly List<Transform> _pieces = new List<Transform>(); // All children that have a PolygonCollider

    private Sprite _intactObjectSprite;
    
    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            
            // Only add the child if it has a polygon collider
            if (child.TryGetComponent(out PolygonCollider2D _)) 
                _pieces.Add(child);
        }

        _intactObjectSprite = GetComponent<SpriteRenderer>().sprite;
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
            // Enable the collider of the new broken piece
            var polygonCollider2D = piece.GetComponent<PolygonCollider2D>();
            polygonCollider2D.enabled = true;

            // Enable the rigidbody of the piece
            piece.GetComponent<Rigidbody2D>().isKinematic = false;
            
            // Create a new sprite renderer
            var spriteRenderer = piece.gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            spriteRenderer.sprite = _intactObjectSprite;
            
            // Shadow caster 2D has horrible support. Use extensions to define the shape
            
            var pointsInPath3D = new Vector3[polygonCollider2D.points.Length];
            
            // Convert Vector2[] to Vector3[]
            for (int j = 0; j < polygonCollider2D.points.Length; ++j)
            {
                pointsInPath3D[j] = polygonCollider2D.points[j]; 
            }
            
            var shadowCaster2D = piece.gameObject.AddComponent<ShadowCaster2D>();

            shadowCaster2D.selfShadows = generateSelfShadowsForPieces;
            
            shadowCaster2D.SetPath(pointsInPath3D.ToArray());
            shadowCaster2D.SetPathHash(Random.Range(int.MinValue, int.MaxValue)); // Hash set initiates internal recalculation
            
        }
        
        if(destroyOriginalObject)
            Destroy(gameObject);
    }
}
