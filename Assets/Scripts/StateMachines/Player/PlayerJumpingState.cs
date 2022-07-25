using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private Vector2 Momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        Momentum = stateMachine.Controller.velocity;
        Momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.JumpHash, CrossFadeDuration);

        //stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(Momentum, deltaTime);
        Debug.Log("Jumping: "+stateMachine.Controller.velocity.y );
        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFreeFallingState(stateMachine));
            return;
        }

        FaceTarget();
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
