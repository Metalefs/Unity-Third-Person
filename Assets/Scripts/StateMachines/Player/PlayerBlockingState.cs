using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ShieldDefense.IsActive = true;
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.BlockingBlendTreeHash, 0.1f, 0);
        SubscribeToInputEvents();
    }

    public override void Tick(float deltaTime)
    {
        Movement(deltaTime);
        if (!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.ReturnToLastState();
            return;
        }
         if(stateMachine.InputReader.MovementValue == Vector2.zero){
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.BlockingSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.BlockingSpeedHash, 1, AnimatorDampTime, deltaTime);
    }

    private void Movement(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + movement * stateMachine.FreeLookMovementSpeed / 2) * deltaTime);
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 0, 0.1f, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 1, 0.1f, deltaTime);
        FaceMovementDirection(movement, deltaTime);
        FaceTarget();        
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
        stateMachine.ShieldDefense.IsActive = false;
        UnsubscribeFromInputEvents();
    }
}
