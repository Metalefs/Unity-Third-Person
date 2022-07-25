using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private GroundRayCast GroundRayCast;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private ShieldDefense ShieldDefense;
    [SerializeField] private float Drag = 0.3f;
    private float VerticalVelocity;
    private Vector3 Impact;
    private Vector3 DampingVelocity;
    public Vector3 Movement => Impact + Vector3.up * VerticalVelocity;

    private void Update()
    {        
        if (VerticalVelocity < 0f && (Controller.isGrounded || GroundRayCast.IsGrounded()))
        {
            VerticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Impact = Vector3.SmoothDamp(Impact, Vector3.zero, ref DampingVelocity, Drag);

        if (Agent != null)
        {
            if (Impact.sqrMagnitude < 0.2f * 0.2f)
            {
                Impact = Vector3.zero;
                Agent.enabled = true;
            }
        }
    }

    public void Reset()
    {
        Impact = Vector3.zero;
        VerticalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        Impact += force;
        if (Agent != null)
        {
            Agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        VerticalVelocity += jumpForce;
    }

}
