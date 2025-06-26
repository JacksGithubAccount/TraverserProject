using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TraverserProject
{

    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;


        [Header("Test")]
        [SerializeField]
        bool defeatedBossDebug = false;
    


    public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsServer)
            {
                if (!WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated = WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated[bossID];

                    if (hasBeenDefeated)
                    {
                        aiCharacterNetworkManager.isActive.Value = false;
                    }
                }
            }

        }



        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated = true;

                if (!WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                else
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldSaveGameManager.Singleton.SaveGame();
            }

            yield return new WaitForSeconds(5);


        }

    }
}