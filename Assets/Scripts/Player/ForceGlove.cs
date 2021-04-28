using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGlove : MonoBehaviour
{
    [SerializeField] private float pushRadius;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private LayerMask whatToPush;

    
    public bool HasGlove  { private set; get; }

    public void EquipGlove()
    {
        HasGlove = true;
    }

    public void Push()
    {
        if(!HasGlove) return;
        var foundColliders2D = Physics2D.OverlapCircleAll((Vector2) transform.position, pushRadius, whatToPush);
        foreach (var var in foundColliders2D)
        {
            var forceApplier = var.gameObject.AddComponent<ForceApplier>();
            forceApplier.affectedRigidBody2D = var.GetComponent<Rigidbody2D>();
            var forceVector = var.transform.position - transform.position;
            forceVector.Normalize();
            forceVector *= forceMultiplier;
            forceApplier.affectVector = forceVector;
            forceApplier.forceMode2D = ForceMode2D.Impulse;
            forceApplier.Affect();
        }
    }
}
