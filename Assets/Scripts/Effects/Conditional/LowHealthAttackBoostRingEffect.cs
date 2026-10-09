using TraverserProject;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Conditional Effects/Low Health Attack Boost Ring")]
public class LowHealthAttackBoostRingEffect : ConditionalCharacterEffect
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

    [Header("VFX")]
    public GameObject VFX;

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

        VFX.SetActive(true);
        VFX = Instantiate(VFX);
        VFX.transform.position = character.characterEffectsManager.effectTransform.position;
        VFX.transform.root.rotation = Quaternion.identity;
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

        VFX.SetActive(false);
    }
    public override bool CheckIfMeetsConditionals(CharacterManager character)
    {

        return AmountOfHealthLeftToTriggerEffect > (character.characterNetworkManager.currentHealth.Value / character.characterNetworkManager.maxHealth.Value) * 100;
    }
}
