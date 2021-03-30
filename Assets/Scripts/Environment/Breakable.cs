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

    // Maximum value of render layers id's. Used to calculate a unique render layer on the fly for masking
    private const int MAXRenderLayer = 32767; 
    
    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            
            // Only add the child if it has a polygon collider and a sprite mask
            if (child.TryGetComponent(out PolygonCollider2D _) && child.TryGetComponent(out SpriteMask _)) 
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
            
            /*
             The piece actually has the whole original object as the sprite. It is just masked with the broken piece to 
             achieve the illusion of being broken. However, the rendering layer has to be unique. Otherwise pieces 
             with overlapping sprites could render each other.
            */
            
            int uniqueRenderingLayer = Random.Range(0, MAXRenderLayer);
            spriteRenderer.sortingOrder = uniqueRenderingLayer;
            
            var spriteMask = piece.gameObject.GetComponent<SpriteMask>();
            spriteMask.isCustomRangeActive = true; // Custom range is used to render every piece independently
            spriteMask.frontSortingOrder = uniqueRenderingLayer;
            spriteMask.backSortingOrder = uniqueRenderingLayer - 1;

            piece.gameObject.AddComponent<AutomaticShadowCaster2D>();
        }
        
        if(destroyOriginalObject)
            Destroy(gameObject);
    }
}
