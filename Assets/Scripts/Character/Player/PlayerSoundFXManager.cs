using UnityEngine;

namespace TraverserProject
{

    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        public override void PlayBlockSoundFX()
        {
            PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));
        }
        public override void PlayFootStepSoundFX()
        {
            base.PlayFootStepSoundFX();
            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 2);
        }

    }
}