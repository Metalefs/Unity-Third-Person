using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Combat
{
    public class ShieldDefense : MonoBehaviour
    {
        public bool IsActive { get; set; }
        [SerializeField] private float DefensePercentage = 0.5f;
        [SerializeField] private float KnockbackDefensePercentage = 0.5f;

        public int ReduceDamage(ref int damage)
        {
            if (!IsActive) { return damage; }
            int damageReduced = (int)Mathf.Round(damage * DefensePercentage);
            damage -= damageReduced;
            return damageReduced;
        }

        public Vector3 ReduceKnockback(ref Vector3 force)
        {
            if (!IsActive) { return force; }
            Vector3 knockbackReduced = force * KnockbackDefensePercentage;
            force -= knockbackReduced;
            return knockbackReduced;
        }
    }
}