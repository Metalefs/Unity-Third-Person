using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private float Drag = 0.5f;
    private float VerticalVelocity;
    private Vector3 Impact;
    private Vector3 DampingVelocity;
    public Vector3 Movement => Impact + Vector3.up * VerticalVelocity;

    private void Update()
    {
        if (VerticalVelocity < 0f && Controller.isGrounded)
        {
            VerticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Impact = Vector3.SmoothDamp(Impact, Vector3.zero, ref DampingVelocity, Drag);
    }

    public void AddForce(Vector3 force)
    {
        Impact += force;
    }
}
