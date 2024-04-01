using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class ResetAnimatorBoolAI : ResetAnimatorBool
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }
    }
}
