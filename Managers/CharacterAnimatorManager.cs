using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ZV
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager characterManager;
        protected CharacterStatsManager characterStatsManager;
        public bool canRotate;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootMotion(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }

        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanBeRiposted()
        {
            characterManager.canBeReposted = true;
        }

        public virtual void DisableCanBeReposted()
        {
            characterManager.canBeReposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            characterStatsManager.TakeDamageNoAnimation(characterManager.pendindCriticalDamage);
            characterManager.pendindCriticalDamage = 0;
        }

        public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
        {
            // CHECK IF WE ARE TWO HANDING OUR WEAPON
            if (isTwoHandingWeapon)
            {
                rightHandConstraint.data.target = rightHandTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;

                leftHandConstraint.data.target = leftHandTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
            else 
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }

        public virtual void EraseHandIKForWeapon()
        { 
            // RESET WEIGHTS TO 0
        }
    }
}
