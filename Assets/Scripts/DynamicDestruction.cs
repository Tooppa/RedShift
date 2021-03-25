using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
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
            if (child.TryGetComponent(out PolygonCollider2D _)) 
                _pieces.Add(child);
        }
        
        _sprite = GetComponent<SpriteRenderer>().sprite;
    }
    
    public void BreakApart()
    {
        foreach (var piece in _pieces)
        {
            // Instantiate new piece without parenting. The new piece has exactly the same location as the original object
            var newPiece = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
        
            // Set the just instantiated collider's sprite to match the original
            // This will be masked by the pre-defined piece's shape
            newPiece.GetComponent<SpriteRenderer>().sprite = _sprite;
            
            // Move the pre-defined piece to mask the new piece
            piece.SetParent(newPiece.transform);
            
            // The new piece's collider will be defined by the pre-defined piece. Enable it
            newPiece.transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;

        }

        // Destroy the original object
        Destroy(gameObject);
    }
}
