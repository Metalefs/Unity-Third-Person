using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }
    [field: SerializeField] public float DodgeCooldown { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }
    [field: SerializeField] public ShieldDefense ShieldDefense { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public GroundRayCast GroundRayCast { get; private set; }
    public Transform MainCameraTransform  { get; private set; }
    public bool IsDead = false;
    public float PreviousDodgeTime {get; private set;} = Mathf.NegativeInfinity;
    
    void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        if(!ShieldDefense.IsActive)
            SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        IsDead = true;
        if(Animator.HasState(0, PlayerAnimatorHashes.DeadStateHash))
        {
            Animator.Play(PlayerAnimatorHashes.DeadStateHash, 0, 0);
        }
        Targeter.enabled = false;
        SwitchState(new PlayerDeadState(this));
    }

    public void SetDodgeTime(float time)
    {
        PreviousDodgeTime = time;
    }
}
