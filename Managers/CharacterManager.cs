using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeReposted;
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;

        // Damage will be inflicted during an animation event
        // Used in backstab or riposte animations
        public int pendindCriticalDamage;
    }
}
