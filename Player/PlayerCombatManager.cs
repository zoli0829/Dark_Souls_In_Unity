using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class PlayerCombatManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;
        CameraHandler cameraHandler;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attack Animations")]
        string oh_light_attack_01 = "oh_light_attack_01";
        string oh_light_attack_02 = "oh_light_attack_02";
        string oh_light_attack_03 = "oh_light_attack_03";
        string oh_light_attack_04 = "oh_light_attack_04";

        string oh_heavy_attack_01 = "oh_heavy_attack_01";

        string th_light_attack_01 = "th_light_attack_01";
        string th_light_attack_02 = "th_light_attack_02";
        string th_light_attack_03 = "th_light_attack_03";

        string th_heavy_attack_01 = "th_heavy_attack_01";

        string weapon_art = "weapon_art";


        public string lastAttack;

        LayerMask backStabLayer = 1 << 12;
        LayerMask riposteLayer = 1 << 13;

        private void Awake()
        {
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            inputHandler = GetComponent<InputHandler>();
            cameraHandler = FindFirstObjectByType<CameraHandler>();
        }

        public void HandleRBAction()
        {
            playerAnimatorManager.EraseHandIKForWeapon();

            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword 
                || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster
                || playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster
                || playerInventoryManager.rightWeapon.weaponType == WeaponType.PyroCaster)
            {
                PerformMagicAction(playerInventoryManager.rightWeapon, false);
            }
        }

        public void HandleLBAction()
        {
            if(playerManager.isTwoHandingWeapon)
            {
                if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if(playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield ||
                   playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
                {
                    PerformLBBlockingAction();
                }
                else if(playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
                    playerInventoryManager.leftWeapon.weaponType == WeaponType.PyroCaster)
                {
                    PerformMagicAction(playerInventoryManager.leftWeapon, true);
                    playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
                }
            }
        }

        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield
                || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                // do a light attack
            }
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStatsManager.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);

                if (lastAttack == oh_light_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
                }
                else if(lastAttack == th_light_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
                }
                else if(lastAttack == th_light_attack_02)
                {
                    playerAnimatorManager.PlayTargetAnimation(th_light_attack_03, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
                lastAttack = th_light_attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
                lastAttack = oh_light_attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStatsManager.currentStamina <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                // SAME AS LIGHT ATTACK BUT NEED ANIMATIONS
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
                lastAttack = oh_heavy_attack_01;
            }
        }

        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            // PLAY FX
            playerEffectsManager.PlayWeaponFX(false);
        }

        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            playerAnimatorManager.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        private void PerformLBAimingAction()
        {
            playerAnimatorManager.animator.SetBool("isAiming", true);
        }

        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting)
                return;

            if(weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;

            if(isTwoHanding)
            {
                
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager,playerManager.isUsingLeftHand);
        }

        public void AttemptBackstabOrRipose()
        {
            // Check if we have stamina, if we do not, return
            if (playerStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    // CHECK FOR TEAM ID (So we cant back stab friends)
                    // Pull us into a transform behind the enemy so the back stab looks clean
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;

                    // rotate us towards that transform
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendindCriticalDamage = criticalDamage;

                    // play animation
                    playerAnimatorManager.PlayTargetAnimation("Back Stab", true);

                    // make enemy play animation
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);

                    // do damage


                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                // CHECK FOR TEAM ID 
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null && enemyCharacterManager.canBeReposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendindCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Reposted", true);
                }
            }
        }
    }
}
