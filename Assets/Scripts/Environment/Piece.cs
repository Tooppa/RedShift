using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class Piece : MonoBehaviour
{
    /// <summary>
    /// Enables the rigidbody, collider and sprite renderer of the piece.
    /// </summary>
    public void Enable()
    {
        // Enable the collider, rigidbody and the sprite renderer of the new broken piece
            
        var polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonCollider2D.enabled = true;

        GetComponent<Rigidbody2D>().isKinematic = false;

        GetComponent<SpriteRenderer>().enabled = true;
        
        gameObject.AddComponent<AutomaticShadowCaster2D>();
    }
}
