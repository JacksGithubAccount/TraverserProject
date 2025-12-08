using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Music")]
        [SerializeField] AudioClip bossIntroClip;
        [SerializeField] AudioClip bossBattleLoopClip;

        [Header("Status")]
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] List<FogWallInteractable> fogWalls;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;


        [Header("States")]
        public BossSleepState sleepState;

        [Header("Phase Shift")]
        public float minimumHealthPercentageToShift = 50;
        [SerializeField]
        string phaseShiftAnimation = "Phase_Change_01";
        [SerializeField] CombatStanceState phase02CombatStanceState;

        [Header("Defeat")]
        [SerializeField] string defeatMessage =  "GREAT FOE FELLED";

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
                    sleepState.hasBeenAwakened = hasBeenAwakened.Value;

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
                animator.Play(sleepAnimation);
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
            PlayerUIManager.Singleton.playerUIPopUpManager.SendBossDefeatedPopUp(defeatMessage);
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                bossFightIsActive.Value = false;

                foreach (var fogWall in fogWalls)
                {
                    fogWall.isActive.Value = false;
                }

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
                aiCharacterNetworkManager.isAwake.Value = true;
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
                WorldSoundFXManager.Singleton.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

                GameObject bossHealthBar = Instantiate(PlayerUIManager.Singleton.playerUIHudManager.bossHealthBarObject, PlayerUIManager.Singleton.playerUIHudManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossHPBar(this);
                PlayerUIManager.Singleton.playerUIHudManager.currentBossHealthBar = bossHPBar;
            }
            else
            {
                WorldSoundFXManager.Singleton.StopBossMusic();

            }
        }

        public void PhaseShift()
        {
            characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
            combatStance = Instantiate(phase02CombatStanceState);
            currentState = combatStance;
        }

        public override void ActivateCharacter(PlayerManager player)
        {
            if (hasBeenDefeated.Value)
            {
                DeactivateCharacter(player);
                return;
            }
            aiCharacterCombatManager.AddPlayerToPlayersWithinRange(player);

            if (player.IsLocalPlayer)
            {
                //optionally enable renderers or disable for other players not near ai
            }

            if (!NetworkManager.Singleton.IsHost)
                return;

            if (aiCharacterCombatManager.playersWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }
    }
}