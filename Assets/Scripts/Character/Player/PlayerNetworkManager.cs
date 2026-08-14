using System.Collections.Generic;
using System.Globalization;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;
        public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Site Of Grace")]
        public NetworkVariable<int> lastSiteOfGraceUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flasks")]
        public NetworkVariable<int> remainingHealthFlasks = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> remainingFocusPointsFlasks = new NetworkVariable<int>(2, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChugging = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Actions")]
        public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Body")]
        public NetworkVariable<int> hairStyleID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorRed = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorGreen = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> hairColorBlue = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Equipment")]
        public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentSpellID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentQuickSlotItemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Two Handing")]
        public NetworkVariable<int> currentWeaponBeingTwoHanded = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingRightWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isTwoHandingLeftWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Spells")]
        public NetworkVariable<bool> isChargingRightSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingLeftSpell = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Armor")]
        public NetworkVariable<bool> isMale = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> headEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> bodyEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> handEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> legEquipmentID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> accessoryEquipment01ID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> accessoryEquipment02ID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> accessoryEquipment03ID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> accessoryEquipment04ID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Projectiles")]
        public NetworkVariable<int> mainProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> secondaryProjectileID = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAiming = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);

            if (player.isDead.Value)
                player.playerCombatManager.CreateDeadSpot(player.transform.position, player.playerStatsManager.bubbles);

            if (player.isDead.Value && NetworkManager.Singleton.IsServer)
            {
                if (PlayerUIManager.Singleton.playerUIHudManager.currentBossHealthBar != null)
                    PlayerUIManager.Singleton.playerUIHudManager.currentBossHealthBar.RemoveHPBar(1f);

                //if like elden ring, disable all boss fight
                WorldAIManager.Singleton.DisableAllBossFights();
                //also kick all players from world

                poisonBuildUp.Value = 0;
                bleedBuildUp.Value = 0;
                frostBuildUp.Value = 0;
            }
        }

        public override void OnIsBloodLossChanged(bool oldStatus, bool newStatus)
        {
            if (isBloodLoss.Value)
            {
                GameObject bloodLossVFX = Instantiate(WorldCharacterEffectsManager.Singleton.bloodLossVFX);
                if (character.characterEffectsManager.effectTransform != null)
                {
                    bloodLossVFX.transform.parent = character.characterEffectsManager.effectTransform;
                }
                else
                {
                    bloodLossVFX.transform.parent = character.characterCombatManager.lockOnTransform;
                }
                bloodLossVFX.transform.localPosition = Vector3.zero;
                bloodLossVFX.transform.localRotation = Quaternion.identity;

                if (player.IsOwner)
                {
                    PlayerUIManager.Singleton.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Bleed);
                    isBloodLoss.Value = false;
                }
            }
            else
            {
                bleedBuildUp.Value = 0;
            }



        }

        public override void OnIsPoisonedChanged(bool oldStatus, bool newStatus)
        {
            if (player.IsOwner)
            {
                if (isPoisoned.Value)
                {
                    PlayerUIManager.Singleton.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Poison);
                    PlayerUIManager.Singleton.playerUIHudManager.healthBar.ToggleBarFillColor(true);
                }
                else
                {
                    PlayerUIManager.Singleton.playerUIHudManager.healthBar.ToggleBarFillColor(false);
                }
            }

            if (isPoisoned.Value)
            {
                if (character.characterEffectsManager.poisonedVFX != null)
                    return;

                GameObject poisonVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonedVFX);
                if (character.characterEffectsManager.effectTransform != null)
                {
                    poisonVFX.transform.parent = character.characterEffectsManager.effectTransform;
                }
                else
                {
                    poisonVFX.transform.parent = character.characterCombatManager.lockOnTransform;
                }
                poisonVFX.transform.localPosition = Vector3.zero;
                //poisonVFX.transform.localRotation = Quaternion.identity;
                character.characterEffectsManager.poisonedVFX = poisonVFX;
            }
            else
            {
                poisonBuildUp.Value = 0;
                if (character.characterEffectsManager.poisonedVFX == null)
                    return;

                Destroy(character.characterEffectsManager.poisonedVFX);
            }
        }

        public override void OnIsFrostbiteChanged(bool oldStatus, bool newStatus)
        {
            if (isFrostbite.Value)
            {
                if (player.IsOwner)
                {
                    PlayerUIManager.Singleton.playerUIPopUpManager.SendStatusEffectPopUp(BuildUp.Frost);
                }

                if (character.characterEffectsManager.frostbiteVFX != null)
                    return;

                GameObject frostVFX = Instantiate(WorldCharacterEffectsManager.Singleton.frostbiteVFX);
                if (character.characterEffectsManager.effectTransform != null)
                {
                    frostVFX.transform.parent = character.characterEffectsManager.effectTransform;
                }
                else
                {
                    frostVFX.transform.parent = character.characterCombatManager.lockOnTransform;
                }
                frostVFX.transform.localPosition = Vector3.zero;
                //frostVFX.transform.localRotation = Quaternion.identity;
                character.characterEffectsManager.frostbiteVFX = frostVFX;
            }
            else
            {
                frostBuildUp.Value = 0;
                if (character.characterEffectsManager.frostbiteVFX == null)
                    return;

                //option 1
                Destroy(character.characterEffectsManager.frostbiteVFX);


                //option 2
                //Create a script on VFX and call function to "end" it and stop particles so they fade
                // and dont stop suddenly then when faded destroy it
            }
        }

        public void OnIsSneakingChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isSneaking", isSneaking.Value);
        }

        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if (rightHandedAction)
            {
                isUsingLeftHand.Value = false;
                isUsingRightHand.Value = true;
            }
            else
            {
                isUsingLeftHand.Value = true;
                isUsingRightHand.Value = false;
            }
        }

        public void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }
        public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }

        public void SetNewMaxFocusPointValue(int oldMind, int newMind)
        {
            maxFocusPoints.Value = player.playerStatsManager.CalculateFocusPointsBasedOnMindLevel(newMind);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxFocusPointsValue(maxFocusPoints.Value);
            currentFocusPoints.Value = maxFocusPoints.Value;
        }

        public void ChangeMaxHealthValue(int oldMaxHealth, int newMaxHealth)
        {
            maxHealth.Value = newMaxHealth;
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        }

        public void ChangeMaxStaminaValue(int oldMaxStamina, int newMaxStamina)
        {
            maxStamina.Value = newMaxStamina;
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        }

        public void ChangeMaxFocusPointValue(int oldMaxFocusPoint, int newMaxFocusPoint)
        {
            maxFocusPoints.Value = newMaxFocusPoint;
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxFocusPointsValue(maxFocusPoints.Value);
        }

        public void SetNewMaxImmunityBuildUpCapacityValue(int oldVitality, int newVitality)
        {
            immunityBuildUpCapacity.Value = player.playerStatsManager.CalculateBuildUpCapacityBasedOnVigorLevelAndEquipment(newVitality);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxImmunityBuildUpCapacityValue(Mathf.RoundToInt(immunityBuildUpCapacity.Value));
        }
        public void SetNewMaxRobustnessBuildUpCapacityValue(int oldEndurace, int newEndurance)
        {
            robustnessBuildUpCapacity.Value = player.playerStatsManager.CalculateBuildUpCapacityBasedOnEnduranceLevelAndEquipment(newEndurance);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxRobustnessBuildUpCapacityValue(Mathf.RoundToInt(robustnessBuildUpCapacity.Value));
        }
        public void SetNewMaxFocusBuildUpCapacityValue(int oldMind, int newMind)
        {
            focusBuildUpCapacity.Value = player.playerStatsManager.CalculateBuildUpCapacityBasedOnMindLevelAndEquipment(newMind);
            PlayerUIManager.Singleton.playerUIHudManager.SetMaxFocusBuildUpCapacityValue(Mathf.RoundToInt(focusBuildUpCapacity.Value));
        }

        public void OnHairStyleIDChange(int oldID, int newID)
        {
            player.playerBodyManager.ToggleHairType(hairStyleID.Value);
        }

        public void OnHairColorRedChange(float oldID, float newID)
        {
            player.playerBodyManager.SetHairColor();
        }
        public void OnHairColorGreenChange(float oldID, float newID)
        {
            player.playerBodyManager.SetHairColor();
        }
        public void OnHairColorBlueChange(float oldID, float newID)
        {
            player.playerBodyManager.SetHairColor();
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            if (player.IsOwner)
            {
                WeaponItem newWeapon = Instantiate(WorldItemDatabase.Singleton.GetWeaponByID(newID));
                player.playerInventoryManager.currentRightHandWeapon = newWeapon;
            }
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.Singleton.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);

                if (player.playerInventoryManager.currentRightHandWeapon.weaponClass == WeaponClass.Bow)
                {
                    PlayerUIManager.Singleton.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
                }
                else
                {
                    PlayerUIManager.Singleton.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
                }
            }
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            if (player.IsOwner)
            {
                WeaponItem newWeapon = Instantiate(WorldItemDatabase.Singleton.GetWeaponByID(newID));
                player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            }
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.Singleton.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);

                if (player.playerInventoryManager.currentLeftHandWeapon.weaponClass == WeaponClass.Bow)
                {
                    PlayerUIManager.Singleton.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(true);
                }
                else
                {
                    PlayerUIManager.Singleton.playerUIHudManager.ToggleProjectileQuickSlotsVisibility(false);
                }
            }
        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.Singleton.GetWeaponByID(newID));
            player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

            if (player.IsOwner)
                return;

            if (player.playerCombatManager.currentWeaponBeingUsed != null)
                player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnimator);
        }

        public void OnCurrentSpellIDChange(int oldID, int newID)
        {
            SpellItem newSpell = null;

            if (WorldItemDatabase.Singleton.GetSpellByID(newID))
                newSpell = Instantiate(WorldItemDatabase.Singleton.GetSpellByID(newID));

            if (newSpell != null)
            {
                player.playerInventoryManager.currentSpell = newSpell;

                if (player.IsOwner)
                    PlayerUIManager.Singleton.playerUIHudManager.SetSpellQuickSlotIcon(newID);
            }
        }

        public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
        {
            QuickSlotItem newQuickSlotItem = null;

            if (WorldItemDatabase.Singleton.GetQuickSlotItemByID(newID))
                newQuickSlotItem = Instantiate(WorldItemDatabase.Singleton.GetQuickSlotItemByID(newID));

            if (newQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;
            }
            else
            {
                player.playerInventoryManager.currentQuickSlotItem = null;
            }
            PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

        }

        public void OnMainProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Singleton.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Singleton.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.mainProjectile = newProjectile;

            if (player.IsOwner)
                PlayerUIManager.Singleton.playerUIHudManager.SetMainProjectileQuickSlotIcon(player.playerInventoryManager.mainProjectile);


        }

        public void OnSecondaryProjectileIDChange(int oldID, int newID)
        {
            RangedProjectileItem newProjectile = null;

            if (WorldItemDatabase.Singleton.GetProjectileByID(newID))
                newProjectile = Instantiate(WorldItemDatabase.Singleton.GetProjectileByID(newID));

            if (newProjectile != null)
                player.playerInventoryManager.secondaryProjectile = newProjectile;

            if (player.IsOwner)
                PlayerUIManager.Singleton.playerUIHudManager.SetSecondaryProjectileQuickSlotIcon(player.playerInventoryManager.secondaryProjectile);
        }

        public void OnFocusPointsChanged(int oldFP, int newFP)
        {
            if (player.IsOwner)
                PlayerUIManager.Singleton.playerUIHudManager.SetNewFocusPointValue(oldFP, newFP);
        }

        public void OnMaxFocusPointsChanged(int oldFP, int newFP)
        {
            if (player.IsOwner)
                PlayerUIManager.Singleton.playerUIHudManager.SetMaxFocusPointsValue(newFP);
        }



        public void OnIsAimingChanged(bool oldStatus, bool newStatus)
        {
            if (!isAiming.Value)
            {
                PlayerCamera.Singleton.cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Singleton.cameraObject.fieldOfView = 60;
                PlayerCamera.Singleton.cameraObject.nearClipPlane = 0.3f;
                PlayerCamera.Singleton.cameraPivotTransform.localPosition = new Vector3(0, PlayerCamera.Singleton.cameraPivotYPositionOffset, 0);
                PlayerCamera.Singleton.ResetCameraZPosition();
                PlayerUIManager.Singleton.playerUIHudManager.crossHair.SetActive(false);
            }
            else
            {
                PlayerCamera.Singleton.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Singleton.cameraPivotTransform.localEulerAngles = new Vector3(0, 0, 0);
                PlayerCamera.Singleton.cameraObject.fieldOfView = 40;
                PlayerCamera.Singleton.cameraObject.nearClipPlane = 1.1f;
                PlayerCamera.Singleton.cameraPivotTransform.localPosition = Vector3.zero;
                PlayerUIManager.Singleton.playerUIHudManager.crossHair.SetActive(true);
            }
        }

        public void OnIsChargingRightSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChargingRightSpell", isChargingRightSpell.Value);
        }

        public void OnIsChargingLeftSpellChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChargingLeftSpell", isChargingLeftSpell.Value);
        }

        public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsBlockingChanged(oldStatus, newStatus);

            if (IsOwner)
            {
                player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
                player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
                player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
                player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
                player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
                player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
            }
        }

        public void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingWeapon.Value)
            {
                if (IsOwner)
                {
                    isTwoHandingLeftWeapon.Value = false;
                    isTwoHandingRightWeapon.Value = false;
                }

                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerEffectsManager.RemoveStaticEffect(WorldCharacterEffectsManager.Singleton.twoHandingEffect.staticEffectID);
            }
            else
            {
                StaticCharacterEffect twoHandEffect = Instantiate(WorldCharacterEffectsManager.Singleton.twoHandingEffect);
                player.playerEffectsManager.AddStaticEffect(twoHandEffect);
            }

            player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon.Value);
        }

        public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingRightWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
            player.playerEquipmentManager.TwoHandRightWeapon();
        }

        public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
        {
            if (!isTwoHandingLeftWeapon.Value)
                return;

            if (IsOwner)
            {
                currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
                isTwoHandingWeapon.Value = true;
            }
            player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerEquipmentManager.TwoHandLeftWeapon();
        }

        public void OnIsChuggingChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isChugging", isChugging.Value);
        }

        public void OnHeadEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            HeadEquipmentItem equipment = WorldItemDatabase.Singleton.GetHeadEquipmentByID(headEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHeadEquipment(null);
            }
        }

        public void OnBodyEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            BodyEquipmentItem equipment = WorldItemDatabase.Singleton.GetBodyEquipmentByID(bodyEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadBodyEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadBodyEquipment(null);
            }
        }

        public void OnHandEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            HandEquipmentItem equipment = WorldItemDatabase.Singleton.GetHandEquipmentByID(handEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadHandEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadHandEquipment(null);
            }
        }

        public void OnLegEquipmentChanged(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            LegEquipmentItem equipment = WorldItemDatabase.Singleton.GetLegEquipmentByID(legEquipmentID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadLegEquipment(Instantiate(equipment));
            }
            else
            {
                player.playerEquipmentManager.LoadLegEquipment(null);
            }
        }

        public void OnAccessory01Changed(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            AccessoryEquipmentItem equipment = WorldItemDatabase.Singleton.GetAccessoryByID(accessoryEquipment01ID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(Instantiate(equipment), 1);
            }
            else
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(null, 1);
            }
        }

        public void OnAccessory02Changed(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            AccessoryEquipmentItem equipment = WorldItemDatabase.Singleton.GetAccessoryByID(accessoryEquipment02ID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(Instantiate(equipment), 2);
            }
            else
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(null, 2);
            }
        }

        public void OnAccessory03Changed(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            AccessoryEquipmentItem equipment = WorldItemDatabase.Singleton.GetAccessoryByID(accessoryEquipment03ID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(Instantiate(equipment), 3);
            }
            else
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(null, 3);
            }
        }

        public void OnAccessory04Changed(int oldValue, int newValue)
        {
            if (IsOwner)
                return;

            AccessoryEquipmentItem equipment = WorldItemDatabase.Singleton.GetAccessoryByID(accessoryEquipment04ID.Value);

            if (equipment != null)
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(Instantiate(equipment), 4);
            }
            else
            {
                player.playerEquipmentManager.LoadAccessoryEquipment(null, 4);
            }
        }

        public void OnIsMaleChanged(bool oldStatus, bool newStatus)
        {
            player.playerBodyManager.ToggleBodyType(isMale.Value);
        }


        [ServerRpc]
        public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
        {
            if (IsServer)
            {
                NotifyTheServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
            }
        }
        [ClientRpc]
        private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformWeaponBasedAction(actionID, weaponID);
            }
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.Singleton.GetWeaponItemActionByID(actionID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(player, WorldItemDatabase.Singleton.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("Action is null");
            }
        }

        [ClientRpc]
        public override void DestroyAllCurrentActionFXClientRpc()
        {
            if (player.characterEffectsManager.activeSpellWarmUpFX != null)
                Destroy(player.characterEffectsManager.activeSpellWarmUpFX);

            if (player.characterEffectsManager.activeDrawnProjectileFX != null)
                Destroy(player.characterEffectsManager.activeDrawnProjectileFX);

            if (player.characterEffectsManager.activeQuickSlotItemFX != null)
                Destroy(player.characterEffectsManager.activeQuickSlotItemFX);

            if (hasArrowNotched.Value)
            {

                Animator bowAnimator;

                if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
                {
                    bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
                }
                else
                {
                    bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
                }

                bowAnimator.SetBool("isDrawn", false);
                bowAnimator.Play("Bow_Draw_01");

                if (player.IsOwner)
                    hasArrowNotched.Value = false;
            }


        }

        //draw projectile
        [ServerRpc]
        public void NotifyTheServerOfDrawnProjectileServerRpc(int projectileID)
        {
            if (IsServer)
            {
                NotifyTheServerOfDrawnProjectileClientRpc(projectileID);
            }
        }

        [ClientRpc]
        private void NotifyTheServerOfDrawnProjectileClientRpc(int projectileID)
        {
            Animator bowAnimator;

            if (isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Draw_01");

            GameObject arrow = Instantiate(WorldItemDatabase.Singleton.GetProjectileByID(projectileID).drawProjectileModel, player.playerEquipmentManager.leftHandWeaponSlot.transform);
            player.playerEffectsManager.activeDrawnProjectileFX = arrow;


            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.notchArrowSFX));
        }

        //release projectile
        [ServerRpc]
        public void NotifyTheServerOfReleasedProjectileServerRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (IsServer)
            {
                NotifyTheServerOfReleasedProjectileClientRpc(playerClientID, projectileID, xPosition, yPosition, zPosition, yCharacterRotation);
            }
        }

        [ClientRpc]
        public void NotifyTheServerOfReleasedProjectileClientRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            if (playerClientID != NetworkManager.Singleton.LocalClientId)
                PerformReleasedProjectileFromRpc(projectileID, xPosition, yPosition, zPosition, yCharacterRotation);

        }

        private void PerformReleasedProjectileFromRpc(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)
        {
            RangedProjectileItem projectileItem = null;
            if (WorldItemDatabase.Singleton.GetProjectileByID(projectileID) != null)
                projectileItem = WorldItemDatabase.Singleton.GetProjectileByID(projectileID);

            if (projectileItem == null)
                return;

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            projectileInstantiationLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            //aiming
            if (player.playerNetworkManager.isAiming.Value)
            {
                projectileGameObject.transform.LookAt(new Vector3(xPosition, yPosition, zPosition));
            }
            else
            {
                //locked onto target
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - projectileGameObject.transform.position);
                    projectileGameObject.transform.rotation = arrowRotation;
                }
                //unlocked and not aiming
                else
                {
                    player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, yCharacterRotation, player.transform.rotation.z);
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                    projectileGameObject.transform.rotation = arrowRotation;
                }
            }


            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnore = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);

            foreach (Collider hitBox in collidersArrowWillIgnore)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;
        }

        [ServerRpc]
        public void HideWeaponsServerRpc()
        {
            if (IsServer)
                HideWeaponsClientRpc();
        }

        [ClientRpc]
        private void HideWeaponsClientRpc()
        {
            if (player.playerEquipmentManager.rightHandWeaponModel != null)
                player.playerEquipmentManager.rightHandWeaponModel.SetActive(false);

            if (player.playerEquipmentManager.leftHandWeaponModel != null)
                player.playerEquipmentManager.leftHandWeaponModel.SetActive(false);
        }

        [ServerRpc]
        public void NotifyTheServerOfQuickSlotItemActionServerRpc(ulong clientID, int quickSlotID)
        {
            NotifyTheServerOfQuickSlotItemActionClientRpc(clientID, quickSlotID);
        }

        [ClientRpc]
        private void NotifyTheServerOfQuickSlotItemActionClientRpc(ulong clientID, int quickSlotID)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                QuickSlotItem item = WorldItemDatabase.Singleton.GetQuickSlotItemByID(quickSlotID);
                item.AttemptToUseItem(player);
            }
        }
    }
}