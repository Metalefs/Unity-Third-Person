using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRayCast : MonoBehaviour
{
    [SerializeField] private Collider Collider;
    [SerializeField] private float offset;
    [SerializeField] private float radius;
    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position + Vector3.up * offset, Vector3.down * radius, Color.red);
        return Physics.Raycast(transform.position + Vector3.up * offset, Vector3.down, radius);
    }

     void OnDrawGizmos(){
        Debug.DrawRay(transform.position + Vector3.up * offset, Vector3.down * radius, Color.red);
     }
}
