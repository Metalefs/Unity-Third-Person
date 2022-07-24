using UnityEngine;

public abstract class PlayerBaseState : State
{
    public float Speed = 1f;
    
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
        //stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.LookEvent += OnLook;
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public void UnsubscribeFromInputEvents()
    {
        //stateMachine.InputReader.JumpEvent -= OnJump;        
        stateMachine.InputReader.LookEvent -= OnLook;
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    private void OnLook()
    {
        if (Speed > 0) return;
        var CharacterRotation = Camera.main.transform.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;

        stateMachine.transform.rotation = CharacterRotation;

        //turn right animation = 0
        if(Camera.main.transform.transform.rotation.x > 0)
        {
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 0, AnimatorDampTime, 0);
        }
        //turn left animation = 1
        else
        {
            stateMachine.Animator.SetFloat(PlayerAnimatorHashes.FreeLookSpeedHash, 1, AnimatorDampTime, 0);
        }
        if (!stateMachine.Animator.IsInTransition(0))
        {
            stateMachine.Animator.CrossFadeInFixedTime(PlayerAnimatorHashes.TurningBlendTreeHash, CrossFadeDuration);
        }
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
