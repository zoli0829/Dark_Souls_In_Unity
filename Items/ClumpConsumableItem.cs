using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpConsumableItem : ConsumableItem
    {
        [Header("Recovery FX")]
        public GameObject clumpConsumeFX;

        [Header("Cure FX")]
        public bool cureBleed;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = clumpConsumeFX;
            playerEffectsManager.instantiatedFXModel = clump;

            // BLEEDING NOT IMPLEMENTED YET
            if(cureBleed)
            {
                //playerEffectsManager.bleedBuildUp = 0;
                //playerEffectsManager.bleedAmount = playerEffectsManager.defaultBleedAmount;
                //playerEffectsManager.isBleeding = false;

                if(playerEffectsManager.currentParticleFX != null)
                {
                    Destroy(playerEffectsManager.currentParticleFX);
                }
            }

            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}
