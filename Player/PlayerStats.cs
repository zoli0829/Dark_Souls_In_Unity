using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;

        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;
        PlayerAnimatorManager animatorHandler;

        [SerializeField] float staminaRegenerationAmount = 25;
        [SerializeField] float staminaRegenTimer = 0; 

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindAnyObjectByType<HealthBar>();
            staminaBar = FindAnyObjectByType<StaminaBar>();
            focusPointBar = FindAnyObjectByType<FocusPointBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoins(currentFocusPoints);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable)
                return;

            base.TakeDamage(damage, damageAnimation = "Damage_01");
            healthBar.SetCurrentHealth(currentHealth);
            animatorHandler.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                // HANDLE PLAYER DEATH
            }
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);

            if (currentStamina <= 0)
            {
                currentStamina = 0;
            }
        }

        public void RegenerateStamina()
        {
            if(playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;

                if(currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth =+ healAmount;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints -= focusPoints;

            if(currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoins(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            soulCount += souls;
        }
    }
}
