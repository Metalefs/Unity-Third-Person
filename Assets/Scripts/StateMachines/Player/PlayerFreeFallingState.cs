using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeFallingState : PlayerBaseState
{
    private Vector3 momentum;
    public PlayerFreeFallingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FallingIdleHash, CrossFadeDuration);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
        Debug.Log("Falling momentum: " + momentum);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }


    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        
        if (stateMachine.GroundRayCast.IsGrounded())
        {
            stateMachine.SwitchState(new PlayerLandingState(stateMachine));
            return;
        }

        FaceTarget();
    }


    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    } 
}
