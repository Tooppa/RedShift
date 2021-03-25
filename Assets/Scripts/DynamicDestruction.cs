using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Create shadow casters
[RequireComponent(typeof(SpriteRenderer))]
public class DynamicDestruction : MonoBehaviour
{
    public GameObject brokenObjectPrefab;

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
            if (child.TryGetComponent(out PolygonCollider2D collider)) 
                _pieces.Add(child);
        }
        
        _sprite = GetComponent<SpriteRenderer>().sprite;

        BreakApart();
    }
    
    private void BreakApart()
    {
        foreach (var piece in _pieces)
        {
            var pieceColliderPoints = piece.GetComponent<PolygonCollider2D>().points;

            // Instantiate new piece without parenting. The new piece has exactly the same location as the original object
            var newPiece = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
        
            // Set the just instantiated collider's sprite to match the original. This will be automatically masked by it's child
            newPiece.GetComponent<SpriteRenderer>().sprite = _sprite;
        
            // The new piece also has a child object that has the mask and the collider.
            // Set it's position and rotation to match the pre-defined piece
            var newPiecesChild = newPiece.transform.GetChild(0);
            newPiecesChild.position = piece.position;
            newPiecesChild.rotation = piece.rotation;
        
            // Collider points match the one found in the pre-defined piece
            newPiecesChild.GetComponent<PolygonCollider2D>().points = pieceColliderPoints; 
        }
        

    }
}
