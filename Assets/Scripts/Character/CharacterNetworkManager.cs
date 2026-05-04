using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Target")]
        public NetworkVariable<ulong> currentTargetNetworkObjectID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isBlocking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParrying = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isParryable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isAttacking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isInvulnerable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isLockedOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isSneaking = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isHidden = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isChargingAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isRipostable = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isBeingCriticallyDamaged = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isRolling = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isPoisoned = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isBloodLoss = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isFrostbite = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isFrozen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isExitingLadder = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isClimbingLadder = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //for ladder2
        public NetworkVariable<bool> isSlidingDownLadder = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); //for ladder2


        [Header("Resources")]
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> currentHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> currentFocusPoints = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxFocusPoints = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Stats")]
        public NetworkVariable<int> vigor = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> mind = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> strength = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> dexterity = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> intelligence = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> faith = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> luck = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Build Ups/Status Effects")]
        public NetworkVariable<float> immunityBuildUpCapacity = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> robustnessBuildUpCapacity = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> focusBuildUpCapacity = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> poisonBuildUp = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> bleedBuildUp = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> frostBuildUp = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Stats Modifiers")]
        public NetworkVariable<int> strengthModifier = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> staminaRegenerationModifier = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> armorPhysicalDamageAbsorptionModifer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> armorMagicDamageAbsorptionModifer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> armorFireDamageAbsorptionModifer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> armorLightningDamageAbsorptionModifer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> armorHolyDamageAbsorptionModifer = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void CheckHealth(int oldValue, int newValue)
        {
            if (currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());

            }

            if (character.IsOwner)
            {
                if (currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }

        public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)

        {
            character.animator.SetBool("isDead", character.isDead.Value);

            if (IsOwner)
            {
                character.characterCombatManager.SetTarget(null);

            }
        }

        public virtual void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            if (!IsOwner)
                character.characterCombatManager.currentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
        }

        public void OnIsLockedOnChanged(bool old, bool isLockedOn)
        {
            if (!isLockedOn)
                character.characterCombatManager.currentTarget = null;
        }

        public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isChargingAttack", isChargingAttack.Value);
        }

        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isMoving", isMoving.Value);
        }

        public void OnIsClimbingLadderChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isClimbingLadder", isClimbingLadder.Value);
            if (isClimbingLadder.Value)
            {
                character.characterEquipmentManager.HideWeapons();
                character.characterLocomotionManager.ignoreGravity = true;
                //disable any other unrelated IK systems here
            }
            else
            {
                character.characterEquipmentManager.UnHideWeapons();
                character.characterLocomotionManager.ignoreGravity = false;
                //re-enable IK systems disabled above
            }
        }

        public void OnIsSlidingDownLadderChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isSlidingDownLadder", isSlidingDownLadder.Value);

            if (isSlidingDownLadder.Value && character.IsOwner)
                character.characterAnimatorManager.PlayTargetActionAnimation("Slide_Down_Ladder_Start_01", true, true, false, true);
        }

        public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            gameObject.SetActive(isActive.Value);
        }

        public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isBlocking", isBlocking.Value);
        }

        public virtual void OnIsPoisonedChanged(bool oldStatus, bool newStatus)
        {
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
                poisonVFX.transform.localRotation = Quaternion.identity;
            }
            else
            {
                if (character.characterEffectsManager.poisonedVFX == null)
                    return;

                //option 1
                Destroy(character.characterEffectsManager.poisonedVFX);

                //option 2
                //Create a script on VFX and call function to "end" it and stop particles so they fade
                // and dont stop suddenly then when faded destroy it
            }
        }

        public virtual void OnIsBloodLossChanged(bool oldStatus, bool newStatus)
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
            }
        }

        public virtual void OnIsFrostbiteChanged(bool oldStatus, bool newStatus)
        {
            if (isFrostbite.Value)
            {
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
                frostVFX.transform.localRotation = Quaternion.identity;
                character.characterEffectsManager.frostbiteVFX = frostVFX;
            }
            else
            {
                if (character.characterEffectsManager.frostbiteVFX == null)
                    return;

                //option 1
                Destroy(character.characterEffectsManager.frostbiteVFX);

                //option 2
                //Create a script on VFX and call function to "end" it and stop particles so they fade
                // and dont stop suddenly then when faded destroy it
            }
        }

        public virtual void OnIsFrozenChanged(bool oldStatus, bool newStatus)
        {
            if (isFrozen.Value)
            {
                character.animator.speed = 0;
                character.characterEffectsManager.PlayFrozenFX();
            }
            else
            {
                character.animator.speed = 1;
            }
        }

        public virtual void OnIsExitingLadderChanged(bool oldStatus, bool newStatus)
        {
            character.animator.SetBool("isExitingLadder", isExitingLadder.Value);
        }

        [ServerRpc(RequireOwnership = false)]
        public virtual void AddCharacterToListOfCharactersTargetingMeServerRpc(ulong characterTargetingMeID)
        {
            if (IsServer)
                AddCharacterToListOfCharactersTargetingMeClientRpc(characterTargetingMeID);
        }

        [ClientRpc(RequireOwnership = false)]
        protected virtual void AddCharacterToListOfCharactersTargetingMeClientRpc(ulong characterTargetingMeID)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(characterTargetingMeID))
                return;

            CharacterManager characterTargetingMe = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterTargetingMeID].GetComponent<CharacterManager>();

            if (characterTargetingMe == null)
                return;

            if (!character.characterCombatManager.charactersTargetingMe.Contains(characterTargetingMe))
                character.characterCombatManager.charactersTargetingMe.Add(characterTargetingMe);

            character.characterCombatManager.CheckForHiddenStatus();
        }

        [ServerRpc(RequireOwnership = false)]
        public virtual void RemoveCharacterFromListOfCharactersTargetingMeServerRpc(ulong characterTargetingMeID)
        {
            if (IsServer)
                RemoveCharacterFromListOfCharactersTargetingMeClientRpc(characterTargetingMeID);
        }
        [ClientRpc()]
        protected virtual void RemoveCharacterFromListOfCharactersTargetingMeClientRpc(ulong characterTargetingMeID)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(characterTargetingMeID))
                return;

            CharacterManager characterTargetingMe = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterTargetingMeID].GetComponent<CharacterManager>();

            if (characterTargetingMe == null)
                return;

            if (character.characterCombatManager.charactersTargetingMe.Contains(characterTargetingMe))
                character.characterCombatManager.charactersTargetingMe.Remove(characterTargetingMe);

            character.characterCombatManager.CheckForHiddenStatus();
        }

        [ServerRpc]
        public virtual void ClearTargetServerRpc()
        {
            if (IsServer)
                ClearTargetClientRpc();
        }
        [ClientRpc]
        protected virtual void ClearTargetClientRpc()
        {
            if (!IsOwner)
                character.characterCombatManager.currentTarget = null;
        }


        //used to cancel FX when poise broken
        [ServerRpc]
        public void DestroyAllCurrentActionFXServerRpc()
        {
            if (IsServer)
            {
                DestroyAllCurrentActionFXClientRpc();

            }
        }

        [ClientRpc]
        public virtual void DestroyAllCurrentActionFXClientRpc()
        {
            if (character.characterEffectsManager.activeSpellWarmUpFX != null)
                Destroy(character.characterEffectsManager.activeSpellWarmUpFX);

            if (character.characterEffectsManager.activeDrawnProjectileFX != null)
                Destroy(character.characterEffectsManager.activeDrawnProjectileFX);

            if (character.characterEffectsManager.activeQuickSlotItemFX != null)
                Destroy(character.characterEffectsManager.activeQuickSlotItemFX);

        }

        //action animation
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        //instant action animation
        [ServerRpc]
        public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayInstantActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformInstantActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformInstantActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.Play(animationID);
        }

        //attack action animation
        [ServerRpc]
        public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (IsServer)
            {
                PlayAttackActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }

        //damage
        [ServerRpc(RequireOwnership = false)]
        public void NofityTheServerOfCharacterDamageServerRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            if (IsServer)
            {
                NofityTheServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);

            }
        }

        [ClientRpc]
        public void NofityTheServerOfCharacterDamageClientRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            ProcessCharacterDamageFromServer(damagedCharacterID, characterCausingDamageID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);

        }

        public void ProcessCharacterDamageFromServer(ulong damagedCharacterID, ulong characterCausingDamageID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage,
            float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].GetComponent<CharacterManager>();

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        //critical damage (riposte)
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfRiposteServerRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            if (IsServer)
            {
                NotifyTheServerOfRiposteClientRpc(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage);

            }
        }

        [ClientRpc]
        public void NotifyTheServerOfRiposteClientRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            ProcessRiposteFromServer(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage);

        }

        public void ProcessRiposteFromServer(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].GetComponent<CharacterManager>();

            WeaponItem weapon = WorldItemDatabase.Singleton.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);


            StartCoroutine(damagedCharacter.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition
                (characterCausingDamage, WorldUtilityManager.Singleton.GetRipostingPositionBasedOnWeaponClass(weapon.weaponClass)));
        }

        //backstab
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfBackstabServerRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            if (IsServer)
            {
                NotifyTheServerOfBackstabClientRpc(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage);

            }
        }

        [ClientRpc]
        public void NotifyTheServerOfBackstabClientRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            ProcessBackstabFromServer(damagedCharacterID, characterCausingDamageID, criticalDamageAnimation, weaponID, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage);

        }

        public void ProcessBackstabFromServer(ulong damagedCharacterID, ulong characterCausingDamageID,
            string criticalDamageAnimation, int weaponID,
            float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)
        {
            CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].GetComponent<CharacterManager>();

            WeaponItem weapon = WorldItemDatabase.Singleton.GetWeaponByID(weaponID);
            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeCriticalDamageEffect);

            if (damagedCharacter.IsOwner)
                damagedCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value = true;

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffect(damageEffect);
            damagedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly(criticalDamageAnimation, true);


            StartCoroutine(characterCausingDamage.characterCombatManager.ForceMoveEnemyCharacterToBackstabPosition
                (damagedCharacter, WorldUtilityManager.Singleton.GetBackstabbingPositionBasedOnWeaponClass(weapon.weaponClass)));
        }

        //parry
        [ServerRpc(RequireOwnership = false)]
        public void NotifyTheServerOfParryServerRpc(ulong parriedClientID)
        {
            if (IsServer)
            {
                NotifyTheServerOfParryClientRpc(parriedClientID);
            }
        }
        [ClientRpc]
        protected void NotifyTheServerOfParryClientRpc(ulong parriedClientID)
        {
            ProcessParryFromServer(parriedClientID);
        }

        protected void ProcessParryFromServer(ulong parriedClient)
        {
            CharacterManager parriedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[parriedClient].gameObject.GetComponent<CharacterManager>();

            if (parriedCharacter == null)
                return;

            if (parriedCharacter.IsOwner)
            {
                parriedCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parried_01", true);
            }
        }
    }
}