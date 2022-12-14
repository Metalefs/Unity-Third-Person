using UnityEngine;
using Combat;
public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine){}


    public override void Enter()
    {   
        stateMachine.Ragdoll.ToggleRagdoll(true);
        foreach(WeaponDamage weapon in stateMachine.WeaponDamage)
        {
            weapon.gameObject.SetActive(false);
            GameObject.Destroy(stateMachine.Target);
        }
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
