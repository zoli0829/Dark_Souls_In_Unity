using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;

        [Header("Equipment Model Changers")]
        // HEAD EQUIPMENT
        HelmetModelChanger helmetModelChanger;
        // TORSO EQUIPMENT
        TorsoModelChanger torsoModelChanger;
        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        // LEG EQUIPMENT
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        // HAND EQUIPMENT
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        [SerializeField] BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
        }

        private void Start()
        {
            EquipAllEquipmentModelsOnStart();
        }

        private void EquipAllEquipmentModelsOnStart()
        {
            // HEAD EQUIPMENT
            helmetModelChanger.UnEquipAllHelmetModels();

            // Equip helmet equipment
            if(playerInventory.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                // Equip default head
                nakedHeadModel.SetActive(true);
            }

            // TORSO EQUIPMENT
            torsoModelChanger.UnEquipAllTorsoModels();
            upperLeftArmModelChanger.UnEquipAllArmModels();
            upperRightArmModelChanger.UnEquipAllArmModels();

            // Equip torso equipment
            if(playerInventory.currentTorsoEqipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEqipment.torsoModelName);
                upperLeftArmModelChanger.EquiparmModelByName(playerInventory.currentTorsoEqipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquiparmModelByName(playerInventory.currentTorsoEqipment.upperRightArmModelName);
            }
            else
            {
                // Equip default torso (naked)
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                upperLeftArmModelChanger.EquiparmModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquiparmModelByName(nakedUpperRightArm);
            }
            // LEG EQUIPMENT
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLeftLegModels();
            rightLegModelChanger.UnEquipAllRightLegModels();

            // Equip leg equipment
            if(playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegModelName);
            }
            else
            {
                // Equip default leg
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);
            }

            // HAND EQUIPMENT
            lowerLeftArmModelChanger.UnEquipAllArmModels();
            lowerRightArmModelChanger.UnEquipAllArmModels();
            leftHandModelChanger.UnEquipAllArmModels();
            rightHandModelChanger.UnEquipAllArmModels();

            if(playerInventory.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquiparmModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquiparmModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquiparmModelByName(playerInventory.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquiparmModelByName(playerInventory.currentHandEquipment.rightHandModelName);
            }
            else
            {
                lowerLeftArmModelChanger.EquiparmModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquiparmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquiparmModelByName(nakedLeftHand);
                rightHandModelChanger.EquiparmModelByName(nakedRightHand);
            }
        }

        public void OpenBlockingCollider()
        {
            // WILL HAVE TO CHANGE THAT RIGHT WEAPON WILL ALSO CAN BLOCK
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
