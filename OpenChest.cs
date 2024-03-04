using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            // Rotate our player towards the chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            // lock the player's transform to a certain point in front of the chest and plays the player opening chest animation
            playerManager.OpenChestInteraction(playerStandingPosition);

            // open the chest lid 
            animator.Play("Chest Open");

            // spawn an item inside the chest the player can pick up
            StartCoroutine(SpawnItemInChest());

            WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

            if(weaponPickUp != null)
            {
                weaponPickUp.weapon = itemInChest;
            }
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}
