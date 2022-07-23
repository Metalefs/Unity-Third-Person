using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private CharacterController Controller;
    private Collider[] allColliders;
    private Rigidbody[] allRigidBodies;

    void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidBodies = GetComponentsInChildren<Rigidbody>(true);
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach (Rigidbody rigidBody in allRigidBodies)
        {
            if (rigidBody.gameObject.CompareTag("Ragdoll"))
            {
                rigidBody.isKinematic = !isRagdoll;
                rigidBody.useGravity = isRagdoll;
            }
        }
        Controller.enabled = !isRagdoll;
        Animator.enabled = !isRagdoll;
    }
}
