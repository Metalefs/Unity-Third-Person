using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
    
    protected float GetNormalizedTime(Animator Animator) 
    {
        AnimatorStateInfo currentInfo = Animator.GetCurrentAnimatorStateInfo(1);
        AnimatorStateInfo nextInfo = Animator.GetNextAnimatorStateInfo(1);
        
        if (Animator.IsInTransition(1) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!Animator.IsInTransition(1) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
