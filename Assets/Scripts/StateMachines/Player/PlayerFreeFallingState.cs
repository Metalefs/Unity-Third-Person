using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeFallingState : PlayerBaseState
{
    private Vector3 momentum;
    float lastRaycastRadius;

    public PlayerFreeFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FallingIdleHash, CrossFadeDuration);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        lastRaycastRadius = stateMachine.GroundRayCast.radius;
        stateMachine.GroundRayCast.radius = 3f;
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
        stateMachine.GroundRayCast.radius = lastRaycastRadius;
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        Debug.Log("Ledge detected in falling state");
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }

}
