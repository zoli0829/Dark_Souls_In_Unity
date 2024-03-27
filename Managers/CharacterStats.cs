using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class CharacterStats : MonoBehaviour
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

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public bool isDead;

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
    }
}
