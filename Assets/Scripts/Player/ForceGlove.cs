using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGlove : MonoBehaviour
{
    [SerializeField] private float pushRadius;
    [SerializeField] private LayerMask whatToPush;
    
    private Vector2 _pushOffset = new Vector2(0.5f, 0);
    
    public bool HasGlove  { private set; get; }

    public void EquipGlove()
    {
        HasGlove = true;
    }

    public void Push()
    {
        Physics2D.OverlapCircle((Vector2) transform.position + _pushOffset, pushRadius, whatToPush);
    }
}
