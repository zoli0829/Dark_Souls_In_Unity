using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ZV
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerLocomotionManager playerLocomotion;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        protected override void Awake()
        {
            base.Awake();

            cameraHandler = FindFirstObjectByType<CameraHandler>();
            interactableUI = FindFirstObjectByType<InteractableUI>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();

            // moved these below from the start 
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
        }

        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStatsManager.isDead);
            animator.SetBool("isBlocking", isBlocking);
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isAiming = animator.GetBool("isAiming");

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float delta = Time.fixedDeltaTime;

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);

            //playerEffectsManager.HandleAllBuildUpEffects();
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        #region Player Interactions

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactibleText;
                        interactableUI.interactibleText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if(inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero; // stops the player from ice skating
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        #endregion
    }
}
