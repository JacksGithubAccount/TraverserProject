using TraverserProject;
using UnityEngine;

public class LowHealthAttackBoostRing : ConditionalCharacterEffect
{
    [Header("Effect Calculated")]
    private bool effectHasBeenInitialized = false;

    [Header("Damage Modified")]
    public int physicalDamageModified = 0;
    public int magicDamageModifed = 0;
    public int fireDamageModified = 0;
    public int lightningDamageModified = 0;
    public int holyDamageModified = 0;


    [Header("Conditional")]
    public float AmountOfHealthLeftToTriggerEffect = 0;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (effectHasBeenInitialized)
            return;

        if (!CheckIfMeetsConditionals(character))
        {
            RemoveEffect(character);
            return;
        }

        effectHasBeenInitialized = true;

        if (!character.IsOwner)
            return;

        character.characterNetworkManager.physicalDamageModifier.Value += physicalDamageModified;
        character.characterNetworkManager.magicDamageModifier.Value += magicDamageModifed;
        character.characterNetworkManager.fireDamageModifier.Value += fireDamageModified;
        character.characterNetworkManager.lightningDamageModifier.Value += lightningDamageModified;
        character.characterNetworkManager.holyDamageModifier.Value += holyDamageModified;
    }

    public override void RemoveEffect(CharacterManager character)
    {
        base.RemoveEffect(character);

        if (!effectHasBeenInitialized)
            return;

        if (!character.IsOwner)
            return;

        character.characterNetworkManager.physicalDamageModifier.Value -= physicalDamageModified;
        character.characterNetworkManager.magicDamageModifier.Value -= magicDamageModifed;
        character.characterNetworkManager.fireDamageModifier.Value -= fireDamageModified;
        character.characterNetworkManager.lightningDamageModifier.Value -= lightningDamageModified;
        character.characterNetworkManager.holyDamageModifier.Value -= holyDamageModified;
    }
    public override bool CheckIfMeetsConditionals(CharacterManager character)
    {

        return AmountOfHealthLeftToTriggerEffect > (character.characterNetworkManager.currentHealth.Value / character.characterNetworkManager.maxHealth.Value) * 100;
    }
}
