using UnityEngine;

public abstract class PlayerBaseState : State
{
    public PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        SubscribeToInputEvents();
    }

    public void SubscribeToInputEvents()
    {
        //stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public void UnsubscribeFromInputEvents()
    {
        //stateMachine.InputReader.JumpEvent -= OnJump;        
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

     protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }



    public void FaceMovementDirection(Vector3 movement, float deltaTime){
        if(movement == Vector3.zero) return;
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
        if (!stateMachine.Targeter.SelectTarget()) { return; }
        if (stateMachine.currentState is PlayerTargetingState)
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        else
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

}
