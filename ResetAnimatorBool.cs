using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
        }
    }
}
