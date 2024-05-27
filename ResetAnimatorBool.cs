using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        public string isUsingRightHand = "isUsingRightHand";
        public bool isUsingRightHandStatus = false;

        public string isUsingLeftHand = "isUsingLeftHand";
        public bool isUsingLeftHandStatus = false;

        public string isInvulnerable = "isInvulnerable";
        public bool isInvulnerableStatus = false;

        public string isInteractingBool = "isInteracting";
        public bool isInteractingStatus = false;

        public string canRotateBool = "canRotate";
        public bool canRotateStatus = true;

        public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
        public bool isRotatingWithRootMotionStatus = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(isInteractingBool, isInteractingStatus);
            animator.SetBool(canRotateBool, canRotateStatus);
            animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
            animator.SetBool(isInvulnerable, isInvulnerableStatus);
            animator.SetBool(isUsingRightHand, isUsingRightHandStatus);
            animator.SetBool(isUsingLeftHand, isUsingLeftHandStatus);
        }
    }
}
