using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class Breakable : MonoBehaviour
{
    public GameObject brokenObjectPrefab;

    [Tooltip("Shadow casters are generated on the fly. Check this if self shadows should be included along with the default options.")]
    public bool generateSelfShadowsForPieces;

    private readonly List<Transform> _pieces = new List<Transform>(); // All children that have a PolygonCollider
    private Sprite _sprite;  // Used to give the same sprite to the broken pieces

    private void Start()
    {
        if (brokenObjectPrefab == null) // Null references, null references everywhere
        {
            Debug.LogWarning("Broken Object prefab not found for " + gameObject.name);
            Destroy(this);
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            
            // Only add the child if it has a polygon collider
            if (child.TryGetComponent(out PolygonCollider2D _)) 
                _pieces.Add(child);
        }
        
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
    
    /// <summary>
    /// Breaks the object and creates the editor-defined pieces for it. When done with spawning the broken pieces,
    /// the original object will be destroyed.
    /// If there aren't any children with PolygonCollider2D's, nothing would be spawned but the object will still be destroyed.
    /// </summary>
    public void BreakApart()
    {
        foreach (var piece in _pieces)
        {
            // Instantiate new piece without parenting. The new piece has exactly the same location as the original object
            var newPiece = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
        
            // Set the just instantiated collider's sprite to match the original
            // This will be masked by the pre-defined piece's shape
            newPiece.GetComponent<SpriteRenderer>().sprite = _sprite;
            
            // Assign the pre-defined piece to mask the new piece
            piece.SetParent(newPiece.transform);

            var newPiecesChild = newPiece.transform.GetChild(0);
            var polygonCollider2D = newPiecesChild.GetComponent<PolygonCollider2D>();
            
            // The new piece's collider will be defined by the pre-defined piece. Enable it
            polygonCollider2D.enabled = true;

            var pointsInPath3D = new Vector3[polygonCollider2D.points.Length];
            
            // Shadow caster 2D has horrible support. Use extensions to define the shape
            
            // Convert Vector2[] to Vector3[]
            for (int j = 0; j < polygonCollider2D.points.Length; ++j)
            {
                pointsInPath3D[j] = polygonCollider2D.points[j]; 
            }
            
            var shadowCaster2D = newPiecesChild.gameObject.AddComponent<ShadowCaster2D>();

            shadowCaster2D.selfShadows = generateSelfShadowsForPieces;
            
            shadowCaster2D.SetPath(pointsInPath3D.ToArray());
            shadowCaster2D.SetPathHash(Random.Range(int.MinValue, int.MaxValue)); // Hash set initiates internal recalculation

        }

        // Destroy the original object
        Destroy(gameObject);
    }
}
