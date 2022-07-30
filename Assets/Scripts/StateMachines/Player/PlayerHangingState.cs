using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private Vector3 ledgeForward;
    private Vector3 closestPoint;
    public PlayerHangingState(PlayerStateMachine stateMachine,Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine) {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.HangingHash, CrossFadeDuration);
        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.Controller.enabled = true;
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
    }   

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y < 0f){
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFreeFallingState(stateMachine));
            return;
        }
        else if (stateMachine.InputReader.MovementValue.y > 0f)
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
            return;
        }
        Move(deltaTime);
    }

    public override void Exit()
    {
        
    }
}
