using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;

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
            inputHandler = GetComponent<InputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

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

            if(playerInventoryManager.currentHelmetEquipment != null)
            {
                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;

                Debug.Log("Helmet Absorption is " + playerStatsManager.physicalDamageAbsorptionHead + "%");
            }
            else
            {
                // Equip default head
                nakedHeadModel.SetActive(true);

                // Setting the default physical damage absorption
                playerStatsManager.physicalDamageAbsorptionHead = 0;
            }

            // TORSO EQUIPMENT
            torsoModelChanger.UnEquipAllTorsoModels();
            upperLeftArmModelChanger.UnEquipAllArmModels();
            upperRightArmModelChanger.UnEquipAllArmModels();

            if(playerInventoryManager.currentTorsoEqipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEqipment.torsoModelName);
                upperLeftArmModelChanger.EquiparmModelByName(playerInventoryManager.currentTorsoEqipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquiparmModelByName(playerInventoryManager.currentTorsoEqipment.upperRightArmModelName);
                playerStatsManager.physicalDamageAbsorptionBody = playerInventoryManager.currentTorsoEqipment.physicalDefense;

                Debug.Log("Body Absorption is " + playerStatsManager.physicalDamageAbsorptionBody + "%");
            }
            else
            {
                // Equip default torso (naked)
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                upperLeftArmModelChanger.EquiparmModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquiparmModelByName(nakedUpperRightArm);

                // Setting the default physical damage absorption
                playerStatsManager.physicalDamageAbsorptionBody = 0;
            }
            // LEG EQUIPMENT
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLeftLegModels();
            rightLegModelChanger.UnEquipAllRightLegModels();

            if(playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(playerInventoryManager.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(playerInventoryManager.currentLegEquipment.rightLegModelName);
                playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;

                Debug.Log("Leg Absorption is " + playerStatsManager.physicalDamageAbsorptionLegs + "%");
            }
            else
            {
                // Equip default leg
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);

                // Setting the default physical damage absorption
                playerStatsManager.physicalDamageAbsorptionLegs = 0;
            }

            // HAND EQUIPMENT
            lowerLeftArmModelChanger.UnEquipAllArmModels();
            lowerRightArmModelChanger.UnEquipAllArmModels();
            leftHandModelChanger.UnEquipAllArmModels();
            rightHandModelChanger.UnEquipAllArmModels();

            if(playerInventoryManager.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquiparmModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquiparmModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquiparmModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquiparmModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
                playerStatsManager.physicalDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.physicalDefense;

                Debug.Log("Hand Absorption is " + playerStatsManager.physicalDamageAbsorptionHands + "%");
            }
            else
            {
                lowerLeftArmModelChanger.EquiparmModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquiparmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquiparmModelByName(nakedLeftHand);
                rightHandModelChanger.EquiparmModelByName(nakedRightHand);

                // Setting the default physical damage absorption
                playerStatsManager.physicalDamageAbsorptionHands = 0;
            }
        }

        public void OpenBlockingCollider()
        {
            // WILL HAVE TO CHANGE THAT RIGHT WEAPON WILL ALSO CAN BLOCK
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
