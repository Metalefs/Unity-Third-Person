using UnityEngine;
using System.Collections;
public abstract class PlayerBaseState : State
{
    public float AnimatorDampTime = 0.1f;
    public float CrossFadeDuration = 0.5f;
    public PlayerStateMachine stateMachine;
    public bool IsDead { get { return stateMachine.IsDead; } }
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public void SubscribeToInputEvents()
    {
        stateMachine.InputReader.LookEvent += OnLook;
        stateMachine.InputReader.TargetEvent += OnTarget;
    }
    public void UnsubscribeFromInputEvents()
    {
        stateMachine.InputReader.LookEvent -= OnLook;
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }
    private void OnLook()
    {
        var CharacterRotation = Camera.main.transform.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        stateMachine.transform.rotation = stateMachine.InputReader.MovementValue == Vector2.zero ?
            CharacterRotation
            :
            Quaternion.Slerp(stateMachine.Controller.transform.rotation, CharacterRotation, AnimatorDampTime);
        
        // if (stateMachine.currentState is not PlayerFreeLookState) { return; }
        // if (stateMachine.InputReader.MovementValue == Vector2.zero)
        // {
        //     if (GetNormalizedTime(stateMachine.Animator, "Turning") < 1f)
        //     {
        //         int direction = 0;
        //         //turn right animation = 0
        //         if (stateMachine.InputReader.MouseValue.x < 0)
        //             direction = 1;
        //         stateMachine.Animator.SetFloat(PlayerAnimatorHashes.TurningDirectionHash, direction, AnimatorDampTime, Time.deltaTime);
        //         stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.TurningBlendTreeHash, CrossFadeDuration);
        //     }
        //     else
        //     stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FreeLookBlendTreeHash, CrossFadeDuration);
        // }
    }
    protected void OnDodge()
    {
        if (stateMachine.InputReader.MouseValue == Vector2.zero)
            return;
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    public void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (stateMachine.currentState is PlayerTargetingState) return;
        if (movement == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(movement);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, deltaTime * stateMachine.RotationDamping);
    }
    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ReturnToLocomotion()
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

    public void OnTarget()
    {
        if (stateMachine.currentState is PlayerTargetingState)
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        else
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }
}
