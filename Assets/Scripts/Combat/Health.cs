using System;
using UnityEngine;
namespace Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;
        private bool isInvulnerable;

        public event Action OnTakeDamage;
        public event Action OnDie;

        public bool IsDead => health == 0;

        private void Start()
        {
            health = maxHealth;
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            this.isInvulnerable = isInvulnerable;
        }

        public void DealDamage(int damage)
        {
            if (health == 0) { return; }

            if (isInvulnerable) { return; }
            Debug.Log($"{name} took {damage} damage");
            health = Mathf.Max(health - damage, 0);

            OnTakeDamage?.Invoke();

            if (health == 0)
            {
                OnDie?.Invoke();
            }
        }
    }


}
