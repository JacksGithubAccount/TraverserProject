using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{

    public class CharacterEffectsManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Current FX")]
        public GameObject activeQuickSlotItemFX;
        public GameObject activeSpellWarmUpFX;
        public GameObject activeDrawnProjectileFX;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;
        [SerializeField] GameObject criticalBloodSplatterVFX;
        [SerializeField] GameObject healedVFX;

        [Header("Status Effect Transform")]
        [SerializeField] public Transform effectTransform;

        [Header("Status Effect VFX")]
        [HideInInspector] public GameObject poisonedVFX;
        [HideInInspector] public GameObject frostbiteVFX;

        [Header("Static Effects")]
        public List<StaticCharacterEffect> staticEffects = new List<StaticCharacterEffect>();

        [Header("Timed Effects")]
        [SerializeField] protected float effectTickTimer = 0;
        [SerializeField] protected float defaultEffectTickTime = 1;
        public List<TimedCharacterEffect> timedEffects = new List<TimedCharacterEffect>();

        [Header("Renderers")]
        private SkinnedMeshRenderer[] skinnedMeshRenderers;

        private MeshRenderer[] meshRenderers;

        [Header("Frozen")]
        private Coroutine frozenCoroutine;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            effectTickTimer -= Time.deltaTime;

            if (effectTickTimer <= 0)
            {
                effectTickTimer = defaultEffectTickTime;
                ProcessTimedEffects();
            }
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Singleton.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }

        public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
        {
            if (criticalBloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Singleton.criticalBloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
        public void PlayHealedVFX(Vector3 contactPoint)
        {
            if (healedVFX != null)
            {
                GameObject healedEffect = Instantiate(healedVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject healedEffect = Instantiate(WorldCharacterEffectsManager.Singleton.healedVFX, contactPoint, Quaternion.identity);
            }
        }

        public virtual void AddBuildUps(BuildUp buildUpType, float amount)
        {
            if (!character.IsOwner)
                return;

            switch (buildUpType)
            {
                case BuildUp.Poison:
                    character.characterNetworkManager.poisonBuildUp.Value += amount;
                    break;
                case BuildUp.Bleed:
                    character.characterNetworkManager.bleedBuildUp.Value += amount;
                    break;
                case BuildUp.Frost:
                    character.characterNetworkManager.frostBuildUp.Value += amount;
                    break;
                default:
                    break;
            }
        }

        //Static Effects
        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            staticEffects.Add(effect);
            effect.ProcessStaticEffect(character);

            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect effect;

            for (int i = 0; i < staticEffects.Count; i++)
            {
                if (staticEffects[i] != null)
                {
                    if (staticEffects[i].staticEffectID == effectID)
                    {
                        effect = staticEffects[i];
                        effect.RemoveStaticEffect(character);
                        staticEffects.Remove(effect);
                    }
                }
            }

            for (int i = staticEffects.Count - 1; i > -1; i--)
            {
                if (staticEffects[i] == null)
                    staticEffects.RemoveAt(i);
            }

        }

        //Timed Effects

        public void ProcessTimedEffects()
        {
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;

                timedEffects[i].ProcessEffect(character);
            }
        }

        public void AddTimedEffect(TimedCharacterEffect effect)
        {
            bool effectIsAlreadyOnCharacter = false;

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;
                if (timedEffects[i].effectID == effect.effectID)
                {
                    effectIsAlreadyOnCharacter = true;
                    timedEffects[i].timeRemainingOnEffect = timedEffects[i].defaultLengthOfEffect;
                }
            }

            if (!effectIsAlreadyOnCharacter)
            {
                timedEffects.Add(effect);
                effect.timeRemainingOnEffect = effect.defaultLengthOfEffect;

                effect.ProcessEffect(character);
            }
        }

        public void RemoveTimedEffect(int effectID)
        {

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    return;

                if (timedEffects[i].effectID == effectID)
                {
                    TimedCharacterEffect effect = timedEffects[i];
                    effect.RemoveEffect(character);
                    timedEffects.RemoveAt(i);
                }
            }
        }

        public TimedCharacterEffect CheckForTimedEffect(int effectID)
        {
            TimedCharacterEffect timedEffect = null;



            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i].effectID == effectID)
                {
                    timedEffect = timedEffects[i];
                    break;
                }
            }
            return timedEffect;
        }

        public void ProcessEffectDamage(int effectDamage)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            character.characterNetworkManager.currentHealth.Value -= effectDamage;

            if (character.characterNetworkManager.currentHealth.Value >= 1)
                return;

            if (!character.characterNetworkManager.isBeingCriticallyDamaged.Value)
                character.characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);

            character.characterNetworkManager.isPoisoned.Value = false;
            character.isDead.Value = true;
        }

        public void PlayFrozenFX()
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            meshRenderers = GetComponentsInChildren<MeshRenderer>();

            if (frozenCoroutine != null)
                StopCoroutine(frozenCoroutine);

            frozenCoroutine = StartCoroutine(ActivateFrozenVFXCoroutine(WorldUtilityManager.Singleton.GetFrozenMaterial()));
        }

        private IEnumerator ActivateFrozenVFXCoroutine(Material frozenMaterial)
        {
            List<Material> originalSkinMeshMaterials = new List<Material>();
            List<Material> originalMeshMaterials = new List<Material>();

            bool rotationStatusOnFrozen = character.characterLocomotionManager.canRotate;
            bool canMoveStatusOnFrozen = character.characterLocomotionManager.canMove;
            bool isPerformingActionStatusOnFrozen = character.isPerformingAction;

            character.characterLocomotionManager.canRotate = false;
            character.characterLocomotionManager.canMove = false;
            character.isPerformingAction = true;

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                if (skinnedMeshRenderers[i] == null)
                    continue;

                originalSkinMeshMaterials.Add(Instantiate(skinnedMeshRenderers[i].material));
                skinnedMeshRenderers[i].material = Instantiate(frozenMaterial);
            }

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                originalMeshMaterials.Add(Instantiate(meshRenderers[i].material));
                meshRenderers[i].material = Instantiate(frozenMaterial);
            }

            while (character.characterNetworkManager.isFrozen.Value)
            {
                yield return null;
            }

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                for (int j = 0; j < originalSkinMeshMaterials.Count; j++)
                {
                    skinnedMeshRenderers[i].material = originalSkinMeshMaterials[j];
                }
            }

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                for (int j = 0; j < originalMeshMaterials.Count; j++)
                {
                    meshRenderers[i].material = originalMeshMaterials[j];
                }
            }

            character.characterLocomotionManager.canRotate = rotationStatusOnFrozen;
            character.characterLocomotionManager.canMove = canMoveStatusOnFrozen;
            character.isPerformingAction = isPerformingActionStatusOnFrozen;

            //alternative to replacing material is to make a shader with a frozen property which could add layer
            //of ice over the standard material using the shader, then instead of changing materials, set the 
            //frozen variable value to the desired setting and change it back to 0 when unfrozen
        }

    }
}