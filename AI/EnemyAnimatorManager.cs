using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemyManager;
        EnemyEffectsManager enemyEffectsManager;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        }

        public void AwardSoulsOnDeath()
        {
            // Scan for every player in the scene, award them souls, for now only for 1 player
            PlayerStatsManager playerStats = FindFirstObjectByType<PlayerStatsManager>();
            SoulCounter soulCounter = FindFirstObjectByType<SoulCounter>();

            if (playerStats != null)
            {
                playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);

                if (soulCounter != null)
                {
                    soulCounter.SetSoulCountText(playerStats.soulCount);
                }
            }
        }

        public void PlayWeaponTrailFX()
        {
            enemyEffectsManager.PlayWeaponFX(false);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }
    }
}
