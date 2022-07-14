using UnityEngine;

public abstract class PlayerBaseState : State
{
    public float Speed = 1f;
    public PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        SubscribeToInputEvents();
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
        if(Speed > 0) return;
        var CharacterRotation = Camera.main.transform.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;

        stateMachine.transform.rotation = CharacterRotation;
        stateMachine.Targeter.SelectTarget();
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

    public void OnTarget()
    {
        if (stateMachine.currentState is PlayerTargetingState)
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        else
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

}
