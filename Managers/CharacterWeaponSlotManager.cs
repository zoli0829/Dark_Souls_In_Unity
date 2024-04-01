using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        protected virtual void Awake()
        {
            leftHandSlot = GetComponentInChildren<WeaponHolderSlot>();
            rightHandSlot = GetComponentInChildren<WeaponHolderSlot>();
            backSlot = GetComponentInChildren<WeaponHolderSlot>();
        }
    }
}
