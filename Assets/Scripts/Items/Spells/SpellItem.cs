using UnityEngine;

namespace TraverserProject
{

    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass spellClass;

        [Header("Spell Modifiers")]
        public float fullChargeEffectMultiplier = 2;
        public int spellSlotsUsed = 1;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellCastReleaseFX;

        [Header("Animations")]
        [SerializeField] protected string mainHandSpellAnimation;
        [SerializeField] protected string offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip warmUpSoundFX;
        public AudioClip releaseSoundFX;

        public virtual void AttemptToCastSpell(PlayerManager player)
        {

        }

        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {

        }

        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {

        }

        public virtual void InstantiateReleaseFX(PlayerManager player)
        {

        }

        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            return true;
        }

    }
}