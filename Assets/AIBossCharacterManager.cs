using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;
        [SerializeField] int fogWallID = 0;
        [SerializeField] bool hasBeenDefeated = false;
        [SerializeField] bool hasBeenAwakened = false;
        [SerializeField] List<FogWallInteractable> fogWalls;


        [Header("Test")]
        [SerializeField]
        bool defeatedBossDebug = false;

        [Header("Debug")]
        [SerializeField] bool wakeBossUp = false;


        protected override void Update()
        {
            base.Update();

            if (wakeBossUp)
            {
                wakeBossUp = false;
                WakeBoss();
            }
        }

        private void OnEnable()
        {
            Debug.Log("Bossman enabled");
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Debug.Log("Bossman network spawned");
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
                    hasBeenAwakened = WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened[bossID];


                }
                //locate fog walls
                StartCoroutine(GetFogWallsFromWorldObjectManager());


                if (hasBeenAwakened)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }
                if (hasBeenDefeated)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }

                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }

        }


        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.Singleton.fogWalls.Count == 0)
                yield return new WaitForEndOfFrame();

            fogWalls = new List<FogWallInteractable>();

            foreach (var fogWall in WorldObjectManager.Singleton.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                    fogWalls.Add(fogWall);
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

        public void WakeBoss()
        {
            hasBeenAwakened = true;
            if (!WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            else
            {
                WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, true);
            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }
        }

    }
}