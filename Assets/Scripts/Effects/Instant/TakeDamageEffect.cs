using System;
using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]

    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public PhysicalDamageType physicalDamageType = PhysicalDamageType.Regular;


        [Header("Final Damage")]
        protected int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animations")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            PlayDirectionalBasedDamageAnimation(character);

            PlayDamageSFX(character);
            PlayDamageVFX(character);

            //run this after all other functions that would attempt to play an animation upon being damaged & after poise/stance is calculated
            CalculateStanceDamage(character);



        }

        protected virtual void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            if (characterCausingDamage != null)
            {

            }
            Debug.Log("Original Phys Damage: " + physicalDamage + " Type: " + physicalDamageType.ToString());

            switch(physicalDamageType)
            {
                case PhysicalDamageType.Regular:
                    physicalDamage -= (physicalDamage * (character.characterStatsManager.armorPhysicalDamageAbsorption / 100));
                    break;
                case PhysicalDamageType.Blunt:
                    physicalDamage -= (physicalDamage * (character.characterStatsManager.armorBluntDamageAbsorption / 100));
                    break;
                case PhysicalDamageType.Pierce:
                    physicalDamage -= (physicalDamage * (character.characterStatsManager.armorPierceDamageAbsorption / 100));
                    break;
                case PhysicalDamageType.Slash:
                    physicalDamage -= (physicalDamage * (character.characterStatsManager.armorSlashDamageAbsorption / 100));
                    break;
                default:
                    physicalDamage -= (physicalDamage * (character.characterStatsManager.armorPhysicalDamageAbsorption / 100));
                    break;
            }
            
            fireDamage -= (fireDamage * (character.characterStatsManager.armorFireDamageAbsorption / 100));
            magicDamage -= (magicDamage * (character.characterStatsManager.armorMagicDamageAbsorption / 100));
            lightningDamage -= (lightningDamage * (character.characterStatsManager.armorLightningDamageAbsorption / 100));
            holyDamage -= (holyDamage * (character.characterStatsManager.armorHolyDamageAbsorption / 100));

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            Debug.Log("After Calculations Phys Damage: " + physicalDamage + " Type: " + physicalDamageType.ToString());
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            //stores poise daamage taken for other interactions
            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense + character.characterStatsManager.offensivePoiseBonus + character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

        protected void CalculateStanceDamage(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            int stanceDamage = Mathf.RoundToInt(poiseDamage);

            if (aiCharacter != null)
            {
                aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
            }
        }

        protected void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        protected void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntFX();
        }

        protected void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.isDead.Value)
                return;

            if (character.characterNetworkManager.isClimbingLadder.Value)
            {
                //eldenring's method - if hit twice within x time, fall off
                if (character.characterLocomotionManager.canBeKnockedOffLadder)
                {
                    KnockCharacterOffLadder(character);
                    return;
                }
                else
                {
                    character.characterLocomotionManager.EnableCanBeKnockedOffLadderForATime(character.characterLocomotionManager.knockOffLadderWindow);
                }


                //falls if poise is broken
                //if(poiseIsBroken)
                //{
                //	KnockCharacterOffLadder(character);
                //	return;
                //}
            }

            if (poiseIsBroken)
            {
                //front
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                //
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                //back
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
                }
                //left
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
                }
                //right
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
                }
            }
            else
            {
                //front
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                //
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                //back
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);
                }
                //left
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);
                }
                //right
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
                }

            }

            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

            if (poiseIsBroken)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
                character.characterCombatManager.DestroyAllCurrentActionFX();
            }
            else
            {
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true);
            }


        }

        protected void KnockCharacterOffLadder(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Ladder_Fall_Start_01", true);
            character.characterLocomotionManager.isExitingLadder = false;
        }

    }
}