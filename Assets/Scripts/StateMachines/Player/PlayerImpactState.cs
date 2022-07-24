using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private new const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.ImpactHash, CrossFadeDuration);
        SubscribeToInputEvents();
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit() {
        UnsubscribeFromInputEvents();
    }
}
