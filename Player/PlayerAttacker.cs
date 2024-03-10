using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorManager animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        LayerMask backStabLayer = 1 << 12;
        LayerMask riposteLayer = 1 << 13;

        private void Awake()
        {
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.oh_light_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_02, true);
                }
                else if(lastAttack == weapon.th_light_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true);
                }
                else if(lastAttack == weapon.th_light_attack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.th_light_attack_03, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true);
                lastAttack = weapon.th_light_attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.oh_light_attack_01, true);
                lastAttack = weapon.oh_light_attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            // Check if we have stamina, if we do not, return
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                // SAME AS LIGHT ATTACK BUT NEED ANIMATIONS
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
                lastAttack = weapon.oh_heavy_attack_01;
            }
        }

        #region Input Actions

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }

        public void HandleLTAction()
        {
            if(playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if(playerInventory.leftWeapon.isMeleeWeapon)
            {
                // do a light attack
            }
        }

        #endregion

        #region Attack Actions

        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
                return;

            if(weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Shrug", true);
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
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }

        #endregion

        #region Defensive Actions

        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.isBlocking)
                return;

            animatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        #endregion

        public void AttemptBackstabOrRipose()
        {
            // Check if we have stamina, if we do not, return
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

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

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendindCriticalDamage = criticalDamage;

                    // play animation
                    animatorHandler.PlayTargetAnimation("Back Stab", true);

                    // make enemy play animation
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);

                    // do damage


                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                // CHECK FOR TEAM ID 
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

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

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendindCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Reposted", true);
                }
            }
        }
    }
}
