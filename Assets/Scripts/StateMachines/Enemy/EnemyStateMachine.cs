using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator  { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    public GameObject Player { get; private set; }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
