using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isInteracting)
                return this;

            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();

                if (characterStats != null)
                {
                    // CHECK FOR TEAM ID
                    if (characterStats.teamIDNumber != enemyStats.teamIDNumber)
                    {
                        Vector3 targetDirection = characterStats.transform.position - transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                        {
                            enemyManager.currentTarget = characterStats;
                            return pursueTargetState;
                        }
                    }
                }
            }
            #endregion

            #region Handle Switching To Next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}
