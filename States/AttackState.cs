using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class AttackState : State
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhilstAttacking(enemyManager);

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
            }

            if(!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager, enemyManager);
                RollForComboChance(enemyManager);
            }

            if(willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this; // GOES BACK UP TO PERFORM THE COMBO
            }

            return rotateTowardsTargetState;
        }

        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandAction);
            enemyAnimatorManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandAction);
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.animator.SetBool("isUsingRightHand", currentAttack.isRightHandAction);
            enemyAnimatorManager.animator.SetBool("isUsingLeftHand", !currentAttack.isRightHandAction);
            willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorManager.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            // Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if(enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelyhood)
            {
                if(currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}
