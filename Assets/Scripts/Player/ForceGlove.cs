using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGlove : MonoBehaviour
{
    [SerializeField] private float pushRadius;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private LayerMask whatToPush;

    private Animator _animator;
    private static readonly int PushTrigger = Animator.StringToHash("Force Glove Push");

    private ParticleSystem _forcePushEffect;
    private bool _cooldown;

    private void Start()
    {
        _animator = transform.parent.GetChild(1).GetComponent<Animator>();
        _forcePushEffect = transform.GetComponentInChildren<ParticleSystem>();
    }

    public bool HasGlove  { private set; get; }

    public void EquipGlove()
    {
        HasGlove = true;
    }

    public void Push()
    {
        if(!HasGlove) return;
        if (_cooldown) return;

        StartCoroutine(Cooldown(1));
        _animator.SetTrigger(PushTrigger);
        _forcePushEffect.Play();

        var foundColliders2D = Physics2D.OverlapCircleAll((Vector2) transform.position, pushRadius, whatToPush);
        foreach (var var in foundColliders2D)
        {
            if (!var.CompareTag("Enemy"))
                StartCoroutine(ResetMovement(var.attachedRigidbody));
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

    private IEnumerator ResetMovement(Rigidbody2D rigid)
    {
        rigid.isKinematic = false;
        if (rigid.mass > 40)
            rigid.mass /= 1000;

        yield return new WaitForSeconds(.2f);
        while (rigid.velocity.x > .01f || rigid.velocity.y > .01f)
        {
            yield return new WaitForEndOfFrame();
        }

        rigid.mass *= 1000;
    }

    private IEnumerator Cooldown(float cooldownTime)
    {
        //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
        _cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        _cooldown = false;
    }
}
