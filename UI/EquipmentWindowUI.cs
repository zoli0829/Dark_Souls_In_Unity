using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        [SerializeField] HandEquipmentSlotUI[] handEquipmentSlotUIs;

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handEquipmentSlotUIs.Length; i++)
            {
                if (handEquipmentSlotUIs[i].rightHandSlot01)
                {
                    handEquipmentSlotUIs[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (handEquipmentSlotUIs[i].rightHandSlot02)
                {
                    handEquipmentSlotUIs[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (handEquipmentSlotUIs[i].leftHandSlot01)
                {
                    handEquipmentSlotUIs[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    handEquipmentSlotUIs[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }
    }
}
