using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem weaponTrail;

        public void PlayWeaponFX()
        {
            weaponTrail.Stop();

            if(weaponTrail.isStopped )
            {
                weaponTrail.Play();
            }
        }
    }
}
