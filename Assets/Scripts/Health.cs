using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public bool destroyWhenDead;
    
    public UnityEvent onDie;
    
    public float CurrentHealth { get; private set; }
    public bool Dead { get; private set; }
    
    // Start is called before the first frame update
    private void Start() => CurrentHealth = maxHealth;
    
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            onDie.Invoke();
            Dead = true;
            
            if(destroyWhenDead)
                Destroy(gameObject);
        }
    }
}
