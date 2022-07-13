using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field:SerializeField] private float maxHealth = 100f;
    private float health;

    private void Start()
    {
        health = (float)maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (health == 0) {return;}
        health = Mathf.Max(health - damage, 0);
        Debug.Log("health: " + health);
        if (health == 0){
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = (int)maxHealth;
        }
    }
}
