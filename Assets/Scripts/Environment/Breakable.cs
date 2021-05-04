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
            
            // Only add the child if it has a Piece-type component
            if (child.TryGetComponent(out Piece _)) 
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
            piece.GetComponent<Piece>().Enable();
        }
        _pieces[0].GetComponent<AudioSource>().Play();
        
        if(destroyOriginalObject)
            Destroy(gameObject);
    }
}
