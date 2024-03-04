using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            enemyStats.TakeDamageNoAnimation(enemyManager.pendindCriticalDamage);
            enemyManager.pendindCriticalDamage = 0;
        }

        public void AwardSoulsOnDeath()
        {
            // Scan for every player in the scene, award them souls, for now only for 1 player
            PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
            SoulCounter soulCounter = FindFirstObjectByType<SoulCounter>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

                if (soulCounter != null)
                {
                    soulCounter.SetSoulCountText(playerStats.soulCount);
                }
            }
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;
        }
    }
}
