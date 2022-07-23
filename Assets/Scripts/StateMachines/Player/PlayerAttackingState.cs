using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyAppliedForce;
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.WeaponDamage.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Movement(deltaTime);

        float normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {

            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.lastState is not PlayerAttackingState)
            {
                stateMachine.SwitchState(stateMachine.lastState);
            }
            else
            {
                if (stateMachine.Targeter.CurrentTarget != null)
                {
                    stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                }
                else
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                }
            }
        }

        previousFrameTime = normalizedTime;
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
        
        if(stateMachine.lastState is PlayerTargetingState)
            FaceTarget();
        
        FaceMovementDirection(movement, deltaTime);
    }

    private Vector3 CalculateMovement()
    {
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;
        Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        //cannot move backwards when attacking
        if (stateMachine.InputReader.MovementValue.x < 0)
        {
            return Vector3.zero;
        }

        return cameraForward * stateMachine.InputReader.MovementValue.y
        + cameraRight * stateMachine.InputReader.MovementValue.x;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new PlayerAttackingState
            (
                stateMachine,
                attack.ComboStateIndex
            )
        );
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }
    
    public override void Exit()
    {
        UnsubscribeFromInputEvents();
    }
}
