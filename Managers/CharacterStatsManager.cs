using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;
        public int soulsAwardedOnDeath = 50;

        [Header("Poise")]
        public float totalPoiseDefence; // TOTAL poise during damage calculation
        public float offensivePoiseBonus; // The poise we GAIN during an attack with a weapon
        public float armorPoiseBonus; // The poise we GAIN from wearing what ever you have equipped
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public bool isDead;

        private void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
        {
            if(isDead)
                return;

            float totalPhysicalDamageAbsorption = 1 - 
                (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - physicalDamageAbsorptionBody / 100) * 
                (1 - physicalDamageAbsorptionLegs / 100) * 
                (1 - physicalDamageAbsorptionHands / 100);

            physicalDamage -= Mathf.RoundToInt((physicalDamage * totalPhysicalDamageAbsorption));

            Debug.Log("Total Damage Absorption is " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = physicalDamage;

            currentHealth -= Mathf.RoundToInt(finalDamage);

            Debug.Log("Total Damage Dealt " + finalDamage);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }
    }
}
