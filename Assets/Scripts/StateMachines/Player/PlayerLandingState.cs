using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private Vector3 momentum;   
    public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FallingToLandingHash, CrossFadeDuration);

        //stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }


    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);
        ReturnToLocomotion();
    }


   public override void Exit()
    {
        //stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    // private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    // {
    //     stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    // }
}
