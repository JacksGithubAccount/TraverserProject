using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace TraverserProject
{

    public class IllusionaryWall : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> illusionIsBroken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public string illusionaryWallID;

        [Header("Collider")]
        [SerializeField] Collider illusionaryWallCollider;  //the actual collider (not trigger) that toggles when illusion dispells

        [Header("Renderer")]
        [SerializeField] MeshRenderer illusionaryWallRenderer;

        [Header("Players within wall trigger")]
        private List<PlayerManager> playersWithinWallTrigger = new List<PlayerManager>();

        private Coroutine illusionaryWallCheckCoroutine;
        private Coroutine fadeWallCoroutine;

        private void Start()
        {
            illusionaryWallID = gameObject.name;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            illusionIsBroken.OnValueChanged += OnIllusionIsBrokenChanged;
            OnIllusionIsBrokenChanged(false, illusionIsBroken.Value);

            if (IsOwner && WorldSaveGameManager.Singleton.currentCharacterData.illusionaryWallsDispelled.Contains(illusionaryWallID))
                BreakIllusion();
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            illusionIsBroken.OnValueChanged -= OnIllusionIsBrokenChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            //1. check when a damage collider hits wall, open it
            DamageCollider damageCollider = other.GetComponent<DamageCollider>();

            if (damageCollider != null)
                BreakIllusion();

            //2. check when player hits all, but only open if roll or action of some sort
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();

            if (player != null)
                AddPlayerToList(player);


        }

        private void BreakIllusion()
        {
            if (IsOwner)
                illusionIsBroken.Value = true;

        }

        private void AddPlayerToList(PlayerManager player)
        {
            for (int i = 0; i < playersWithinWallTrigger.Count; i++)
            {
                if (playersWithinWallTrigger[i] == null)
                    playersWithinWallTrigger.RemoveAt(i);
            }

            if (!playersWithinWallTrigger.Contains(player))
                playersWithinWallTrigger.Add(player);

            if (illusionaryWallCheckCoroutine != null)
                StopCoroutine(illusionaryWallCheckCoroutine);

            illusionaryWallCheckCoroutine = StartCoroutine(IllusionaryWallCheckCoroutine());

        }
        private void RemovePlayerFromList(PlayerManager player)
        {
            if (playersWithinWallTrigger.Contains(player))
                playersWithinWallTrigger.Remove(player);

            for (int i = 0; i < playersWithinWallTrigger.Count; i++)
            {
                if (playersWithinWallTrigger[i] == null)
                    playersWithinWallTrigger.RemoveAt(i);
            }

            if (playersWithinWallTrigger.Count <= 0 && illusionaryWallCheckCoroutine != null)
                StopCoroutine(illusionaryWallCheckCoroutine);
        }

        private IEnumerator IllusionaryWallCheckCoroutine()
        {
            while (playersWithinWallTrigger.Count > 0)
            {
                for (int i = 0; i < playersWithinWallTrigger.Count; i++)
                {
                    if (playersWithinWallTrigger[i] == null)
                    {
                        playersWithinWallTrigger.RemoveAt(i);
                        continue;
                    }

                    if (!playersWithinWallTrigger[i].gameObject.activeInHierarchy)
                    {
                        RemovePlayerFromList(playersWithinWallTrigger[i]);
                        continue;
                    }

                    if (playersWithinWallTrigger[i].isDead.Value)
                    {
                        playersWithinWallTrigger[i].characterController.enabled = false;
                        RemovePlayerFromList(playersWithinWallTrigger[i]);
                        continue;
                    }

                    if (playersWithinWallTrigger[i].playerNetworkManager.isRolling.Value)
                    {
                        BreakIllusion();
                        playersWithinWallTrigger.Clear();
                    }
                }

                yield return null;
            }
        }

        private void OnIllusionIsBrokenChanged(bool oldStatus, bool newStatus)
        {
            if (!illusionIsBroken.Value)
            {
                illusionaryWallCollider.enabled = true;
            }
            else
            {
                gameObject.layer = 1; //transparent FX layer
                fadeWallCoroutine = StartCoroutine(FadeWallCoroutine());
                illusionaryWallCollider.enabled = false;

                if (IsOwner && !WorldSaveGameManager.Singleton.currentCharacterData.illusionaryWallsDispelled.Contains(illusionaryWallID))
                    WorldSaveGameManager.Singleton.currentCharacterData.illusionaryWallsDispelled.Add(illusionaryWallID);
            }
        }

        private IEnumerator FadeWallCoroutine()
        {
            Material clonedMaterial = Instantiate(illusionaryWallRenderer.material);
            illusionaryWallRenderer.material = clonedMaterial;

            Color newColor = clonedMaterial.color;

            while (newColor.a > 0)
            {
                clonedMaterial.color = newColor;
                newColor.a -= Time.deltaTime;
                yield return null;
            }

            illusionaryWallRenderer.enabled = false;
        }

    }
}