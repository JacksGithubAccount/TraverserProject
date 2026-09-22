using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Character Buff Item")]
    public class CharacterBuffItem : QuickSlotItem
    {
        [Header("VFX")]
        [SerializeField] GameObject visualFX;

        [Header("Timed Effects")]
        [SerializeField] TimedCharacterEffect[] characterEffects;

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            Instantiate(visualFX, player.transform);

            for (int i = 0; i < characterEffects.Length; i++)
            {
                if (characterEffects[i] == null)
                    continue;

                player.playerEffectsManager.AddTimedEffect(Instantiate(characterEffects[i]));
            }
        }


    }
}