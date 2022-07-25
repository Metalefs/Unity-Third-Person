using UnityEngine;
using System.Collections;
public abstract class PlayerBaseState : State
{
    public float Speed = 1f;
    protected Vector2 dodgingDirectionInput;
    protected float remainingDodgeTime;
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
        stateMachine.InputReader.DodgeEvent += OnDodge;
    }

    public void UnsubscribeFromInputEvents()
    {
        stateMachine.InputReader.LookEvent -= OnLook;
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void OnLook()
    {
        if (Speed > 0) return;
        var CharacterRotation = Camera.main.transform.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;

        stateMachine.transform.rotation = CharacterRotation;
        if (stateMachine.Controller.velocity.magnitude <= 0)
        {
            if (stateMachine.currentState is PlayerFreeLookState)
            {
                if (Mathf.Abs(stateMachine.InputReader.MouseValue.x) < 3) return;
                if (!stateMachine.Animator.IsInTransition(0))
                {
                    int direction = 0;
                    //turn right animation = 0
                    if (stateMachine.InputReader.MouseValue.x < 0)
                        direction = 1;
                    stateMachine.Animator.SetFloat(PlayerAnimatorHashes.TurningDirectionHash, direction, AnimatorDampTime, 0);
                }
                AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

                //when turning ends set to FreeLookBlendTreeHash
                if (currentInfo.fullPathHash == PlayerAnimatorHashes.TurningBlendTreeHash && currentInfo.normalizedTime >= 1)
                {
                    stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.FreeLookBlendTreeHash, CrossFadeDuration, 0);
                }
            }
        }
    }

    private void OnDodge()
    {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown) { return; }

        stateMachine.SetDodgeTime(Time.time);
        dodgingDirectionInput = stateMachine.InputReader.MovementValue;
        remainingDodgeTime = stateMachine.DodgeDuration;
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
        if (stateMachine.currentState is PlayerFreeLookState) return;
        if (stateMachine.Targeter.CurrentTarget == null) { return; }
        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0f;
        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    public void OnTarget()
    {
        if (stateMachine.currentState is PlayerTargetingState)
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        else
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
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

}
