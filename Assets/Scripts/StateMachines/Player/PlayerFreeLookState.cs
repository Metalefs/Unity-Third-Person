using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade = true;
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 0);
        if (shouldFade)
            stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FreeLookBlendTreeHash, CrossFadeDuration);
        else
            stateMachine.Animator.Play(PlayerAnimatorHashes.FreeLookBlendTreeHash);

        SubscribeToInputEvents();
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.DodgeEvent += OnDodge;
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    protected void OnJump()
    {
        if (stateMachine.GroundRayCast.IsGrounded())
        {    
            // keep forward movement while in air
            var movement = CalculateMovement();
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine, false, movement));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;
        Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReader.MovementValue.y
        + cameraRight * stateMachine.InputReader.MovementValue.x;
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        UnsubscribeFromInputEvents();
    }
}
