using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Status")]
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallInteractable> fogWalls;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;


        [Header("States")]
        [SerializeField] BossSleepState sleepState;

        protected override void Awake()
        {
            base.Awake();

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, bossFightIsActive.Value);

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;
            }

            if (IsServer)
            {
                if (!WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                else
                {
                    hasBeenDefeated.Value = WorldSaveGameManager.Singleton.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.Singleton.currentCharacterData.bossesAwakened[bossID];


                }
                //locate fog walls
                StartCoroutine(GetFogWallsFromWorldObjectManager());


                if (hasBeenAwakened.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }
                if (hasBeenDefeated.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }

                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }

            if (!hasBeenAwakened.Value)
            {
                characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
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

                bossFightIsActive.Value = false;

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }

                hasBeenDefeated.Value = true;

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
            if (IsOwner)
            {
                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
                }

                bossFightIsActive.Value = true;
                hasBeenAwakened.Value = true;
                currentState = idle;

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

        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (bossFightIsActive.Value)
            {
                GameObject bossHealthBar = Instantiate(PlayerUIManager.Singleton.playerUIHudManager.bossHealthBarObject, PlayerUIManager.Singleton.playerUIHudManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossHPBar(this);
            }
        }
    }
}