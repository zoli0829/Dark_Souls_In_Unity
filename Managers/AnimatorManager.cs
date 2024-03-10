using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", canRotate);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {

        }
    }
}
