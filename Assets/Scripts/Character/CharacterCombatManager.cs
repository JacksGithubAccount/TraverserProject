using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GridBrushBase;

namespace TraverserProject
{

    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Previous Poise Damage Taken")]
        public float previousPoiseDamageTaken;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Attack Flags")]
        public bool canPerformRollingAttack = false;
        public bool canPerformBackstepAttack = false;
        public bool canBlock = true;
        public bool canBeBackstabbed = true;

        [Header("Critical Attack")]
        private Transform riposteReceiverTransform;
        private Transform backstabReceiverTransform;
        [SerializeField] float criticalAttackDistanceCheck = 0.7f;
        public int pendingCriticalDamage;

        [Header("Characters Targeting Me")]
        public List<CharacterManager> charactersTargetingMe = new List<CharacterManager>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                    newTarget.characterNetworkManager.AddCharacterToListOfCharactersTargetingMeServerRpc(character.NetworkObjectId);
                }
                else
                {
                    currentTarget = null;
                }
            }
        }

        public virtual void AttemptCriticalAttack()
        {
            if (character.isPerformingAction)
                return;

            if (character.characterNetworkManager.currentStamina.Value <= 0)
                return;

            RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.lockOnTransform.position, character.transform.TransformDirection(Vector3.forward), criticalAttackDistanceCheck, WorldUtilityManager.Singleton.GetCharacterLayers());

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

                if (targetCharacter != null)
                {
                    if (targetCharacter == character)
                        continue;

                    if (!WorldUtilityManager.Singleton.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                        continue;

                    Vector3 directionFromCharacterToTarget = character.transform.position - targetCharacter.transform.position;
                    float targetViewableAngle = Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                    if (targetCharacter.characterNetworkManager.isRipostable.Value)
                    {
                        if (targetViewableAngle >= -60 && targetViewableAngle <= 60)
                        {
                            AttemptRiposte(hit);
                            return;
                        }
                    }

                    if (targetCharacter.characterCombatManager.canBeBackstabbed)
                    {
                        if (targetViewableAngle <= 180 && targetViewableAngle >= 145)
                        {
                            AttemptBackstab(hit);
                            return;
                        }

                        if (targetViewableAngle >= -180 && targetViewableAngle <= -145)
                        {
                            AttemptBackstab(hit);
                            return;
                        }
                    }
                }
            }
        }

        public virtual void AttemptRiposte(RaycastHit hit)
        {
            Debug.Log("Riposting Target");
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (!targetCharacter.characterNetworkManager.isRipostable.Value)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;


        }

        public virtual void AttemptBackstab(RaycastHit hit)
        {
            Debug.Log("Backstabbing Target");
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;


        }

        public virtual void ApplyCriticalDamage()
        {
            character.characterEffectsManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.lockOnTransform.position);
            character.characterSoundFXManager.PlayCriticalStrikeSoundFX();

            if (character.IsOwner)
                character.characterNetworkManager.currentHealth.Value -= pendingCriticalDamage;
        }

        public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 ripostePosition)
        {
            float timer = 0;

            while (timer < 0.5f)
            {
                timer += Time.deltaTime;


                if (riposteReceiverTransform == null)
                {
                    GameObject riposteTransformObject = new GameObject("Riposte Transform");
                    riposteTransformObject.transform.parent = transform;
                    riposteTransformObject.transform.position = Vector3.zero;
                    riposteReceiverTransform = riposteTransformObject.transform;
                }

                riposteReceiverTransform.localPosition = ripostePosition;
                enemyCharacter.transform.position = riposteReceiverTransform.position;
                transform.rotation = Quaternion.LookRotation(-enemyCharacter.transform.forward);
                yield return null;

            }
        }

        public IEnumerator ForceMoveEnemyCharacterToBackstabPosition(CharacterManager enemyCharacter, Vector3 backstabPosition)
        {
            float timer = 0;

            while (timer < 0.2f)
            {
                timer += Time.deltaTime;


                if (backstabReceiverTransform == null)
                {
                    GameObject backstabTransformObject = new GameObject("Backstab Transform");
                    backstabTransformObject.transform.parent = transform;
                    backstabTransformObject.transform.position = Vector3.zero;
                    backstabReceiverTransform = backstabTransformObject.transform;
                }

                backstabReceiverTransform.localPosition = backstabPosition;
                enemyCharacter.transform.position = backstabReceiverTransform.position;
                transform.rotation = Quaternion.LookRotation(enemyCharacter.transform.forward);
                yield return null;

            }
        }

        public void CheckForHiddenStatus()
        {
            for (int i = 0; i < character.characterCombatManager.charactersTargetingMe.Count; i++)
            {
                if (character.characterCombatManager.charactersTargetingMe[i] == null)
                    character.characterCombatManager.charactersTargetingMe.RemoveAt(i);
            }

            if (!character.IsOwner)
                return;

            if (character.characterCombatManager.charactersTargetingMe.Count > 0)
                character.characterNetworkManager.isHidden.Value = false;

            if (character.characterCombatManager.charactersTargetingMe.Count <= 0)
                character.characterNetworkManager.isHidden.Value = true;
        }

        public void EnableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;
        }

        public void DisableIsInvulnerable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = false;
        }

        public void EnableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = true;
        }

        public void DisableIsParrying()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isParrying.Value = false;
        }

        public void EnableIsRipostable()
        {
            if (character.IsOwner)
                character.characterNetworkManager.isRipostable.Value = true;
        }


        public void EnableCanDoRollingAttack()
        {
            canPerformRollingAttack = true;
        }

        public void DisableCanDoRollingAttack()
        {
            canPerformRollingAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }

        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }

        public virtual void EnableCanDoCombo()
        {

        }

        public virtual void DisableCanDoCombo()
        {

        }

        public virtual void ReleaseArrow()
        {

        }
        public virtual void CloseAllDamageColliders()
        {

        }
        //used to destroy drawn arrows or spell warm up fx when character is poise broken
        public void DestroyAllCurrentActionFX()
        {
            character.characterNetworkManager.DestroyAllCurrentActionFXServerRpc();
        }
    }
}