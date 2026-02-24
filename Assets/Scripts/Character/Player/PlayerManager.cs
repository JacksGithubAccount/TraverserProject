using System.Collections;
using System.Collections.Generic;
using TravserserProject;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TraverserProject
{
    public class PlayerManager : CharacterManager
    {
        [Header("DEBUG MENU")]
        [SerializeField] bool respawnCharacter = false;
        [SerializeField] bool switchRightWeapon = false;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;

        [Header("Area")]
        public WorldLocationSceneSet areaCurrentlyIn;

        [Header("Time Played")]
        [SerializeField] float timePlayed = 0;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerBodyManager = GetComponent<PlayerBodyManager>();
        }

        protected override void Update()
        {
            timePlayed += Time.deltaTime;
            base.Update();

            if (!IsOwner)
                return;


            playerLocomotionManager.HandleAllMovement();

            playerStatsManager.RegenerateStamina();

            DebugMenu();
        }
        protected override void LateUpdate()
        {

            base.LateUpdate();
            PlayerCamera.Singleton.HandleAllCameraActions();
        }

        protected override void OnEnable()
        {
            base.OnEnable();


        }

        protected override void OnDisable()
        {
            base.OnDisable();



        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            if (IsOwner)
            {
                PlayerCamera.Singleton.player = this;
                PlayerInputManager.Singleton.player = this;
                PlayerUIManager.Singleton.localPlayer = this;
                WorldSaveGameManager.Singleton.player = this;

                //updates health or stamina when the stat linked to it changes
                playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusPointValue;

                //updates build up capacity when certain stats change
                playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewMaxImmunityBuildUpCapacityValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxRobustnessBuildUpCapacityValue;
                playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMaxFocusBuildUpCapacityValue;

                //updates ui stat bars when a stat changes
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewFocusPointValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                //updates ui build up bars when build up changes
                playerNetworkManager.poisonBuildUp.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewPoisonBuildUpAmount;
                playerNetworkManager.bleedBuildUp.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewBleedBuildUpAmount;
                playerNetworkManager.frostBuildUp.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewFrostBuildUpAmount;


                playerNetworkManager.SetNewMaxHealthValue(0, playerNetworkManager.vigor.Value);
                playerNetworkManager.SetNewMaxStaminaValue(0, playerNetworkManager.endurance.Value);
                playerNetworkManager.SetNewMaxFocusPointValue(0, playerNetworkManager.mind.Value);

                playerNetworkManager.isAiming.OnValueChanged += playerNetworkManager.OnIsAimingChanged;
            }

            if (!IsOwner)
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;

            //body type
            playerNetworkManager.isMale.OnValueChanged += playerNetworkManager.OnIsMaleChanged;

            //stats
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHealth;
            playerNetworkManager.currentFocusPoints.OnValueChanged += playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged += playerNetworkManager.OnMaxFocusPointsChanged;

            //status effects
            playerNetworkManager.isPoisoned.OnValueChanged += playerNetworkManager.OnIsPoisonedChanged;
            playerNetworkManager.isBloodLoss.OnValueChanged += playerNetworkManager.OnIsBloodLossChanged;
            playerNetworkManager.isFrostbite.OnValueChanged += playerNetworkManager.OnIsFrostbiteChanged;
            playerNetworkManager.isFrozen.OnValueChanged += playerNetworkManager.OnIsFrozenChanged;

            //lock on
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;

            //body
            playerNetworkManager.hairStyleID.OnValueChanged += playerNetworkManager.OnHairStyleIDChange;
            playerNetworkManager.hairColorRed.OnValueChanged += playerNetworkManager.OnHairColorRedChange;
            playerNetworkManager.hairColorGreen.OnValueChanged += playerNetworkManager.OnHairColorGreenChange;
            playerNetworkManager.hairColorBlue.OnValueChanged += playerNetworkManager.OnHairColorBlueChange;

            //equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged += playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged += playerNetworkManager.OnCurrentSpellIDChange;
            playerNetworkManager.isBlocking.OnValueChanged += playerNetworkManager.OnIsBlockingChanged;
            playerNetworkManager.headEquipmentID.OnValueChanged += playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged += playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged += playerNetworkManager.OnHandEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged += playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.mainProjectileID.OnValueChanged += playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged += playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged += playerNetworkManager.OnIsHoldingArrowChanged;
            playerNetworkManager.isChugging.OnValueChanged += playerNetworkManager.OnIsChuggingChanged;

            //spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged += playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged += playerNetworkManager.OnIsChargingLeftSpellChanged;

            //twohanding
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged += playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;
            //flags
            playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;
            playerNetworkManager.isSneaking.OnValueChanged += playerNetworkManager.OnIsSneakingChanged;

            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.Singleton.currentCharacterData);
            }

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

            if (IsOwner)
            {
                playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusPointValue;

                playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewMaxImmunityBuildUpCapacityValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxRobustnessBuildUpCapacityValue;
                playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMaxFocusBuildUpCapacityValue;

                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewFocusPointValue;
                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;

                playerNetworkManager.poisonBuildUp.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewPoisonBuildUpAmount;
                playerNetworkManager.bleedBuildUp.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewBleedBuildUpAmount;
                playerNetworkManager.frostBuildUp.OnValueChanged -= PlayerUIManager.Singleton.playerUIHudManager.SetNewFrostBuildUpAmount;


                playerNetworkManager.isAiming.OnValueChanged -= playerNetworkManager.OnIsAimingChanged;
            }

            if (!IsOwner)
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;

            //body type
            playerNetworkManager.isMale.OnValueChanged -= playerNetworkManager.OnIsMaleChanged;

            //stats
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHealth;
            playerNetworkManager.currentFocusPoints.OnValueChanged -= playerNetworkManager.OnFocusPointsChanged;
            playerNetworkManager.maxFocusPoints.OnValueChanged -= playerNetworkManager.OnMaxFocusPointsChanged;

            //status effects
            playerNetworkManager.isPoisoned.OnValueChanged -= playerNetworkManager.OnIsPoisonedChanged;
            playerNetworkManager.isBloodLoss.OnValueChanged -= playerNetworkManager.OnIsBloodLossChanged;
            playerNetworkManager.isFrostbite.OnValueChanged -= playerNetworkManager.OnIsFrostbiteChanged;
            playerNetworkManager.isFrozen.OnValueChanged -= playerNetworkManager.OnIsFrozenChanged;

            //lock on
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;

            //body
            playerNetworkManager.hairStyleID.OnValueChanged -= playerNetworkManager.OnHairStyleIDChange;
            playerNetworkManager.hairColorRed.OnValueChanged -= playerNetworkManager.OnHairColorRedChange;
            playerNetworkManager.hairColorGreen.OnValueChanged -= playerNetworkManager.OnHairColorGreenChange;
            playerNetworkManager.hairColorBlue.OnValueChanged -= playerNetworkManager.OnHairColorBlueChange;

            //equipment
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentQuickSlotItemID.OnValueChanged -= playerNetworkManager.OnCurrentQuickSlotItemIDChange;
            playerNetworkManager.currentSpellID.OnValueChanged -= playerNetworkManager.OnCurrentSpellIDChange;
            playerNetworkManager.isBlocking.OnValueChanged -= playerNetworkManager.OnIsBlockingChanged;
            playerNetworkManager.headEquipmentID.OnValueChanged -= playerNetworkManager.OnHeadEquipmentChanged;
            playerNetworkManager.bodyEquipmentID.OnValueChanged -= playerNetworkManager.OnBodyEquipmentChanged;
            playerNetworkManager.handEquipmentID.OnValueChanged -= playerNetworkManager.OnHandEquipmentChanged;
            playerNetworkManager.legEquipmentID.OnValueChanged -= playerNetworkManager.OnLegEquipmentChanged;
            playerNetworkManager.mainProjectileID.OnValueChanged -= playerNetworkManager.OnMainProjectileIDChange;
            playerNetworkManager.secondaryProjectileID.OnValueChanged -= playerNetworkManager.OnSecondaryProjectileIDChange;
            playerNetworkManager.isHoldingArrow.OnValueChanged -= playerNetworkManager.OnIsHoldingArrowChanged;
            playerNetworkManager.isChugging.OnValueChanged -= playerNetworkManager.OnIsChuggingChanged;

            //spells
            playerNetworkManager.isChargingRightSpell.OnValueChanged -= playerNetworkManager.OnIsChargingRightSpellChanged;
            playerNetworkManager.isChargingLeftSpell.OnValueChanged -= playerNetworkManager.OnIsChargingLeftSpellChanged;

            //twohanding
            playerNetworkManager.isTwoHandingWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingWeaponChanged;
            playerNetworkManager.isTwoHandingRightWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingRightWeaponChanged;
            playerNetworkManager.isTwoHandingLeftWeapon.OnValueChanged -= playerNetworkManager.OnIsTwoHandingLeftWeaponChanged;

            //flags
            playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
            playerNetworkManager.isSneaking.OnValueChanged -= playerNetworkManager.OnIsSneakingChanged;
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            WorldGameSessionManager.Singleton.AddPlayerToActivePlayersList(this);
            if (!IsServer && IsOwner)
            {
                foreach (var player in WorldGameSessionManager.Singleton.players)
                {
                    if (player != this)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }

                StartCoroutine(EmergeAtMostRecentSiteOfGrace());
            }
        }

        private IEnumerator EmergeAtMostRecentSiteOfGrace()
        {
            PlayerManager hostPlayer = null;

            while (hostPlayer == null)
            {
                for (int i = 0; i < WorldGameSessionManager.Singleton.players.Count; i++)
                {
                    if (WorldGameSessionManager.Singleton.players[i].IsHost)
                    {
                        hostPlayer = WorldGameSessionManager.Singleton.players[i];
                    }

                }
                yield return null;
            }

            WorldObjectManager.Singleton.sitesOfGrace[hostPlayer.playerNetworkManager.lastSiteOfGraceUsed.Value].TeleportToSiteOfGrace();
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SendYouDiedPopUp();
            }

            WorldGameSessionManager.Singleton.WaitThenReviveHost();

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if (IsOwner)
            {
                isDead.Value = false;
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

                playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.isMale = playerNetworkManager.isMale.Value;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;
            currentCharacterData.secondsPlayed = timePlayed;

            //stats
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
            currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;

            currentCharacterData.vigor = playerNetworkManager.vigor.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
            currentCharacterData.mind = playerNetworkManager.mind.Value;
            currentCharacterData.strength = playerNetworkManager.strength.Value;
            currentCharacterData.dexterity = playerNetworkManager.dexterity.Value;
            currentCharacterData.intelligence = playerNetworkManager.intelligence.Value;
            currentCharacterData.faith = playerNetworkManager.faith.Value;
            currentCharacterData.luck = playerNetworkManager.luck.Value;

            currentCharacterData.bubbles = playerStatsManager.bubbles;

            //body
            currentCharacterData.hairStyleID = playerNetworkManager.hairStyleID.Value;
            currentCharacterData.hairColorRed = playerNetworkManager.hairColorRed.Value;
            currentCharacterData.hairColorGreen = playerNetworkManager.hairColorGreen.Value;
            currentCharacterData.hairColorBlue = playerNetworkManager.hairColorBlue.Value;

            currentCharacterData.currentHealthFlaskRemaining = playerNetworkManager.remainingHealthFlasks.Value;
            currentCharacterData.currentFocusPointsFlaskRemaining = playerNetworkManager.remainingFocusPointsFlasks.Value;

            //equipment
            currentCharacterData.headEquipment = playerNetworkManager.headEquipmentID.Value;
            currentCharacterData.bodyEquipment = playerNetworkManager.bodyEquipmentID.Value;
            currentCharacterData.handEquipment = playerNetworkManager.handEquipmentID.Value;
            currentCharacterData.legEquipment = playerNetworkManager.legEquipmentID.Value;

            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
            currentCharacterData.rightWeapon01 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[0]);
            currentCharacterData.rightWeapon02 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[1]);
            currentCharacterData.rightWeapon03 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[2]);

            currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
            currentCharacterData.leftWeapon01 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
            currentCharacterData.leftWeapon02 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
            currentCharacterData.leftWeapon03 = WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlots[2]);

            currentCharacterData.spellIndex = playerInventoryManager.quickSlotItemIndex;
            currentCharacterData.spell01 = WorldSaveGameManager.Singleton.GetSerializableSpellFromSpellItem(playerInventoryManager.spellItemsInQuickSlots[0]);
            currentCharacterData.spell02 = WorldSaveGameManager.Singleton.GetSerializableSpellFromSpellItem(playerInventoryManager.spellItemsInQuickSlots[1]);
            currentCharacterData.spell03 = WorldSaveGameManager.Singleton.GetSerializableSpellFromSpellItem(playerInventoryManager.spellItemsInQuickSlots[2]);


            currentCharacterData.quickSlotIndex = playerInventoryManager.quickSlotItemIndex;
            currentCharacterData.quickSlotItem01 = WorldSaveGameManager.Singleton.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[0]);
            currentCharacterData.quickSlotItem02 = WorldSaveGameManager.Singleton.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[1]);
            currentCharacterData.quickSlotItem03 = WorldSaveGameManager.Singleton.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[2]);

            currentCharacterData.mainProjectile = WorldSaveGameManager.Singleton.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.mainProjectile);
            currentCharacterData.secondaryProjectile = WorldSaveGameManager.Singleton.GetSerializableRangedProjectileFromRangedProjectileItem(playerInventoryManager.secondaryProjectile);

            if (playerInventoryManager.currentSpell != null)
                currentCharacterData.currentSpell = playerInventoryManager.currentSpell.itemID;

            currentCharacterData.weaponsInInventory = new List<SerializableWeapon>();
            currentCharacterData.projectilesInInventory = new List<SerializableRangedProjectile>();
            currentCharacterData.quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            currentCharacterData.headEquipmentInInventory = new List<int>();
            currentCharacterData.bodyEquipmentInInventory = new List<int>();
            currentCharacterData.handEquipmentInInventory = new List<int>();
            currentCharacterData.legEquipmentInInventory = new List<int>();

            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                WeaponItem weaponInInventory = playerInventoryManager.itemsInInventory[i] as WeaponItem;
                HeadEquipmentItem headEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;
                BodyEquipmentItem bodyEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;
                HandEquipmentItem handEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;
                LegEquipmentItem legEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;

                QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemsInInventory[i] as QuickSlotItem;
                RangedProjectileItem projectileInInventory = playerInventoryManager.itemsInInventory[i] as RangedProjectileItem;

                if (weaponInInventory != null)
                    currentCharacterData.weaponsInInventory.Add(WorldSaveGameManager.Singleton.GetSerializableWeaponFromWeaponItem(weaponInInventory));

                if (headEquipmentInInventory != null)
                    currentCharacterData.headEquipmentInInventory.Add(headEquipmentInInventory.itemID);

                if (bodyEquipmentInInventory != null)
                    currentCharacterData.bodyEquipmentInInventory.Add(bodyEquipmentInInventory.itemID);

                if (handEquipmentInInventory != null)
                    currentCharacterData.handEquipmentInInventory.Add(handEquipmentInInventory.itemID);

                if (legEquipmentInInventory != null)
                    currentCharacterData.legEquipmentInInventory.Add(legEquipmentInInventory.itemID);

                if (projectileInInventory != null)
                    currentCharacterData.projectilesInInventory.Add(WorldSaveGameManager.Singleton.GetSerializableRangedProjectileFromRangedProjectileItem(projectileInInventory));

                if (quickSlotItemInInventory != null)
                    currentCharacterData.quickSlotItemsInInventory.Add(WorldSaveGameManager.Singleton.GetSerializableQuickSlotItemFromQuickSlotItem(quickSlotItemInInventory));
            }

        }
        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            playerNetworkManager.isMale.Value = currentCharacterData.isMale;
            playerBodyManager.ToggleBodyType(currentCharacterData.isMale);
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
            timePlayed = currentCharacterData.secondsPlayed;

            //Stats
            playerNetworkManager.vigor.Value = currentCharacterData.vigor;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            playerNetworkManager.mind.Value = currentCharacterData.mind;
            playerNetworkManager.strength.Value = currentCharacterData.strength;
            playerNetworkManager.dexterity.Value = currentCharacterData.dexterity;
            playerNetworkManager.intelligence.Value = currentCharacterData.intelligence;
            playerNetworkManager.faith.Value = currentCharacterData.faith;
            playerNetworkManager.luck.Value = currentCharacterData.luck;



            //moved with implement save/load
            playerNetworkManager.maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vigor.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            playerNetworkManager.maxFocusPoints.Value = playerStatsManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
            playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;
            playerNetworkManager.immunityBuildUpCapacity.Value = playerStatsManager.CalculateBuildUpCapacityBasedOnVigorLevelAndEquipment(playerNetworkManager.vigor.Value);
            playerNetworkManager.robustnessBuildUpCapacity.Value = playerStatsManager.CalculateBuildUpCapacityBasedOnEnduranceLevelAndEquipment(playerNetworkManager.endurance.Value);
            playerNetworkManager.focusBuildUpCapacity.Value = playerStatsManager.CalculateBuildUpCapacityBasedOnMindLevelAndEquipment(playerNetworkManager.mind.Value);

            playerStatsManager.AddBubbles(currentCharacterData.bubbles);

            playerNetworkManager.remainingHealthFlasks.Value = currentCharacterData.currentHealthFlaskRemaining;
            playerNetworkManager.remainingFocusPointsFlasks.Value = currentCharacterData.currentFocusPointsFlaskRemaining;

            //body
            playerNetworkManager.hairStyleID.Value = currentCharacterData.hairStyleID;
            playerNetworkManager.hairColorRed.Value = currentCharacterData.hairColorRed;
            playerNetworkManager.hairColorGreen.Value = currentCharacterData.hairColorGreen;
            playerNetworkManager.hairColorBlue.Value = currentCharacterData.hairColorBlue;


            //equipment
            if (WorldItemDatabase.Singleton.GetHeadEquipmentByID(currentCharacterData.headEquipment))
            {
                HeadEquipmentItem headEquipment = Instantiate(WorldItemDatabase.Singleton.GetHeadEquipmentByID(currentCharacterData.headEquipment));
                playerInventoryManager.headEquipment = headEquipment;
            }
            else
            {
                playerInventoryManager.headEquipment = null;
            }

            if (WorldItemDatabase.Singleton.GetBodyEquipmentByID(currentCharacterData.bodyEquipment))
            {
                BodyEquipmentItem bodyEquipment = Instantiate(WorldItemDatabase.Singleton.GetBodyEquipmentByID(currentCharacterData.bodyEquipment));
                playerInventoryManager.bodyEquipment = bodyEquipment;
            }
            else
            {
                playerInventoryManager.bodyEquipment = null;
            }

            if (WorldItemDatabase.Singleton.GetHandEquipmentByID(currentCharacterData.handEquipment))
            {
                HandEquipmentItem handEquipment = Instantiate(WorldItemDatabase.Singleton.GetHandEquipmentByID(currentCharacterData.handEquipment));
                playerInventoryManager.handEquipment = handEquipment;
            }
            else
            {
                playerInventoryManager.handEquipment = null;
            }

            if (WorldItemDatabase.Singleton.GetLegEquipmentByID(currentCharacterData.legEquipment))
            {
                LegEquipmentItem legEquipment = Instantiate(WorldItemDatabase.Singleton.GetLegEquipmentByID(currentCharacterData.legEquipment));
                playerInventoryManager.legEquipment = legEquipment;
            }
            else
            {
                playerInventoryManager.legEquipment = null;
            }


            //weapons
            playerInventoryManager.weaponsInRightHandSlots[0] = currentCharacterData.rightWeapon01.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[1] = currentCharacterData.rightWeapon02.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[2] = currentCharacterData.rightWeapon03.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[0] = currentCharacterData.leftWeapon01.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[1] = currentCharacterData.leftWeapon02.GetWeapon();
            playerInventoryManager.weaponsInLeftHandSlots[2] = currentCharacterData.leftWeapon03.GetWeapon();

            //spells
            playerInventoryManager.quickSlotSpellIndex = currentCharacterData.spellIndex;
            playerInventoryManager.spellItemsInQuickSlots[0] = currentCharacterData.spell01.GetSpell();
            playerInventoryManager.spellItemsInQuickSlots[1] = currentCharacterData.spell02.GetSpell();
            playerInventoryManager.spellItemsInQuickSlots[2] = currentCharacterData.spell03.GetSpell();
            playerEquipmentManager.LoadSpellItemEquipment(playerInventoryManager.spellItemsInQuickSlots[playerInventoryManager.quickSlotSpellIndex]); //refreshes hud


            //quickslots
            playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotIndex;
            playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();
            playerEquipmentManager.LoadQuickSlotItemEquipment(playerInventoryManager.quickSlotItemsInQuickSlots[playerInventoryManager.quickSlotItemIndex]); //refreshes hud


            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            if (currentCharacterData.rightWeaponIndex >= 0)
            {
                playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex];
                playerNetworkManager.currentRightHandWeaponID.Value = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDatabase.Singleton.unarmedWeapon.itemID;
            }

            playerInventoryManager.leftHandWeaponIndex = currentCharacterData.leftWeaponIndex;
            if (currentCharacterData.leftWeaponIndex >= 0)
            {
                playerInventoryManager.currentLeftHandWeapon = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex];
                playerNetworkManager.currentLeftHandWeaponID.Value = playerInventoryManager.weaponsInLeftHandSlots[currentCharacterData.leftWeaponIndex].itemID;
            }
            else
            {
                playerNetworkManager.currentLeftHandWeaponID.Value = WorldItemDatabase.Singleton.unarmedWeapon.itemID;
            }


            if (WorldItemDatabase.Singleton.GetSpellByID(currentCharacterData.currentSpell))
            {
                SpellItem currentSpell = Instantiate(WorldItemDatabase.Singleton.GetSpellByID(currentCharacterData.currentSpell));
                playerNetworkManager.currentSpellID.Value = currentSpell.itemID;
            }
            else
            {
                playerNetworkManager.currentSpellID.Value = -1;
            }

            for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
            {
                WeaponItem weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weapon);
            }

            for (int i = 0; i < currentCharacterData.headEquipmentInInventory.Count; i++)
            {
                HeadEquipmentItem equipment = WorldItemDatabase.Singleton.GetHeadEquipmentByID(currentCharacterData.headEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.bodyEquipmentInInventory.Count; i++)
            {
                BodyEquipmentItem equipment = WorldItemDatabase.Singleton.GetBodyEquipmentByID(currentCharacterData.bodyEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.legEquipmentInInventory.Count; i++)
            {
                LegEquipmentItem equipment = WorldItemDatabase.Singleton.GetLegEquipmentByID(currentCharacterData.legEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.handEquipmentInInventory.Count; i++)
            {
                HandEquipmentItem equipment = WorldItemDatabase.Singleton.GetHandEquipmentByID(currentCharacterData.handEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            for (int i = 0; i < currentCharacterData.projectilesInInventory.Count; i++)
            {
                RangedProjectileItem projectile = currentCharacterData.projectilesInInventory[i].GetProjectile();
                playerInventoryManager.AddItemToInventory(projectile);
            }

            for (int i = 0; i < currentCharacterData.quickSlotItemsInInventory.Count; i++)
            {
                QuickSlotItem quickSlotItem = currentCharacterData.quickSlotItemsInInventory[i].GetQuickSlotItem();
                playerInventoryManager.AddItemToInventory(quickSlotItem);
            }

            playerEquipmentManager.EquipArmor();

            playerEquipmentManager.LoadMainProjectileEquipment(currentCharacterData.mainProjectile.GetProjectile());
            playerEquipmentManager.LoadSecondaryProjectileEquipment(currentCharacterData.secondaryProjectile.GetProjectile());

        }

        private void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            //sync body type
            playerNetworkManager.OnIsMaleChanged(false, playerNetworkManager.isMale.Value);
            playerNetworkManager.OnHairStyleIDChange(0, playerNetworkManager.hairStyleID.Value);
            playerNetworkManager.OnHairColorRedChange(0, playerNetworkManager.hairColorRed.Value);
            playerNetworkManager.OnHairColorGreenChange(0, playerNetworkManager.hairColorGreen.Value);
            playerNetworkManager.OnHairColorBlueChange(0, playerNetworkManager.hairColorBlue.Value);

            //sync weapons
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            playerNetworkManager.OnCurrentSpellIDChange(0, playerNetworkManager.currentSpellID.Value);

            //Sync armors
            playerNetworkManager.OnHeadEquipmentChanged(0, playerNetworkManager.headEquipmentID.Value);
            playerNetworkManager.OnBodyEquipmentChanged(0, playerNetworkManager.bodyEquipmentID.Value);
            playerNetworkManager.OnHandEquipmentChanged(0, playerNetworkManager.handEquipmentID.Value);
            playerNetworkManager.OnLegEquipmentChanged(0, playerNetworkManager.legEquipmentID.Value);

            //sync projectiles
            playerNetworkManager.OnMainProjectileIDChange(0, playerNetworkManager.mainProjectileID.Value);
            playerNetworkManager.OnSecondaryProjectileIDChange(0, playerNetworkManager.secondaryProjectileID.Value);
            playerNetworkManager.OnIsHoldingArrowChanged(false, playerNetworkManager.isHoldingArrow.Value);

            //sync two hand status
            playerNetworkManager.OnIsTwoHandingRightWeaponChanged(false, playerNetworkManager.isTwoHandingRightWeapon.Value);
            playerNetworkManager.OnIsTwoHandingLeftWeaponChanged(false, playerNetworkManager.isTwoHandingLeftWeapon.Value);

            //sync status effects
            playerNetworkManager.OnIsPoisonedChanged(false, playerNetworkManager.isPoisoned.Value);
            playerNetworkManager.OnIsFrostbiteChanged(false, playerNetworkManager.isFrostbite.Value);

            //sync block status
            playerNetworkManager.OnIsBlockingChanged(false, playerNetworkManager.isBlocking.Value);

            //lock on
            if (playerNetworkManager.isLockedOn.Value)
            {
                playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
            }

        }

        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }
            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }
        }
    }
}