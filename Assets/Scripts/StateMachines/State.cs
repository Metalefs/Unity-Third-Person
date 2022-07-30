using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
    
    protected float GetNormalizedTime(Animator Animator, string Tag) 
    {
        AnimatorStateInfo currentInfo = Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = Animator.GetNextAnimatorStateInfo(0);
        
        if (Animator.IsInTransition(0) && nextInfo.IsTag(Tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!Animator.IsInTransition(0) && currentInfo.IsTag(Tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
