using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ZV
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimationManager;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidBody;

        public bool isPerformingAction;
        public bool isInteracting;
        public float rotationSpeed = 15f;
        public float maximumAggroRadius = 1.5f;

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("AI Settings")]
        public float detectionRadius = 20f;
        // The higher, lower, respectively these angles are, the greater or lower the character's detection field of view
        public float maximumDetectionAngle = 50f;
        public float minimumDetectionAngle = -50f;
        public float currentRecoveryTime = 0f;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyhood;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidBody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimationManager.anim.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimationManager.anim.GetBool("isInteracting");
            isInvulnerable = enemyAnimationManager.anim.GetBool("isInvulnerable");
            canDoCombo = enemyAnimationManager.anim.GetBool("canDoCombo");
            canRotate = enemyAnimationManager.anim.GetBool("canRotate");
            enemyAnimationManager.anim.SetBool("isDead", enemyStats.isDead);
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }
    }
}
