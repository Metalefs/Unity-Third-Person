using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider MyCollider;
    private float Damage;
    private List<Collider> HitColliders = new List<Collider>();

    private void OnEnable()
    {
        HitColliders.Clear();
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") { return; }

        if (HitColliders.Contains(other)) { return; }

        HitColliders.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(Damage);
        }
    }
}
