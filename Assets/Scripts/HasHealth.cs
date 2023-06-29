using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HasHealth : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public UnityEvent<float> healthChanged;
    public UnityEvent<float> damaged;
    public UnityEvent<float> healed;
    public UnityEvent<GameObject> dead;

    private void Clamp()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
        if(!enabled) return;
        health -= damage;
        healthChanged.Invoke(-damage);
        damaged.Invoke(damage);
        Clamp();
        if(health <= 0) dead.Invoke(gameObject);
    }
    
    public virtual void TakeHeal(float heal)
    {
        if(!enabled) return;
        health += heal;
        healthChanged.Invoke(heal);
        healed.Invoke(heal);
        Clamp();
    }
}
