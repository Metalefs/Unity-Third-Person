using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{   
    private readonly Vector3 offset = new Vector3(0f, 2.235f, 0.65f);
    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.PullupHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator,"Climbing") < 1f)
        {
            stateMachine.Controller.enabled = false;
            stateMachine.transform.Translate(offset, Space.Self);
            stateMachine.Controller.enabled = true;
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine,false));
        }
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
