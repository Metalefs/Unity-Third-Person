using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Rigidbody Body;
    [SerializeField] private GroundRayCast GroundRayCast;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private ShieldDefense ShieldDefense;
    [SerializeField] private float Drag = 0.3f;
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;
    public float gravityScale = 5;


    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity < 0f && GroundRayCast.IsGrounded())
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, Drag);

        if (Agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                Agent.enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {   
        if(Body != null)
        Body.AddForce(Physics.gravity * (gravityScale - 1) * Body.mass);
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (Agent != null)
        {
            Agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
        if(Body != null)
            Body.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
    }

}
