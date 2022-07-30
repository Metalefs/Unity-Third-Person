using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{

    protected Vector2 dodgingDirectionInput;
    protected float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirection) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirection;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.DodgeForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(PlayerAnimatorHashes.DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.DodgingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        remainingDodgeTime -= deltaTime;
        if (remainingDodgeTime <= 0f)
            ReturnToLocomotion();
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
