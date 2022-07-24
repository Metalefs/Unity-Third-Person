using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}
    
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FreeLookBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.stabilizeFeet = true;
        SubscribeToInputEvents();
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
        if(Speed < stateMachine.FreeLookMovementSpeed)
        {
            Speed += deltaTime * 4;
        }
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + movement * Speed) * deltaTime);
        if(stateMachine.InputReader.MovementValue == Vector2.zero){
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            Speed = 0;
            return;
        }
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement,deltaTime);
    }
    
    private Vector3 CalculateMovement(){
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
        UnsubscribeFromInputEvents();
    }
}
