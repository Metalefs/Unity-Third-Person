using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private float AnimatorDampTime = 0.1f;
    private float CrossFadeDuration = 0.1f;
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}
    
    public override void Enter()
    {
        Debug.Log("Entering FreeLookState");
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
         if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();
        stateMachine.Controller.Move((stateMachine.ForceReceiver.Movement + movement * stateMachine.FreeLookMovementSpeed) * deltaTime);
        if(stateMachine.InputReader.MovementValue == Vector2.zero){
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement,deltaTime);
    }
    
    private Vector3 CalculateMovement(){
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;
        Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReader.MovementValue.y 
        + cameraRight * stateMachine.InputReader.MovementValue.x;
    }

    public override void Exit()
    {
        UnsubscribeFromInputEvents();
        Debug.Log("Exiting FreeLookState");
    }
}
