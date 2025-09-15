using TraverserProject;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using System.Collections;

namespace TravserserProject
{

    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup[] canvasGroup;

        [Header("Stat Bars")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;
        [SerializeField] UI_StatBar focusPointBar;

        [Header("Runes")]
        [SerializeField] float runeUpdateCountDelayTimer = 2.5f;
        private int pendingRunesToAdd = 0;
        private Coroutine waitThenAddRunesCoroutine;
        [SerializeField] TextMeshProUGUI runesToAddText;
        [SerializeField] TextMeshProUGUI runesCountText;

        [Header("Quick Slots")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;
        [SerializeField] Image spellItemQuickSlotIcon;
        [SerializeField] Image quickSlotItemQuickSlotIcon;
        [SerializeField] TextMeshProUGUI quickSlotItemCount;
        [SerializeField] GameObject projectileQuickSlotsGameObject;
        [SerializeField] Image mainProjectileQuickSlotIcon;
        [SerializeField] TextMeshProUGUI mainProjectileCount;
        [SerializeField] Image secondaryProjectileQuickSlotIcon;
        [SerializeField] TextMeshProUGUI secondaryProjectileCount;

        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;

        [Header("Crosshair")]
        public GameObject crossHair;

        public void ToggleHUD(bool status)
        {
            if (status)
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 1;
                }
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0;
                }
            }
        }

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
            focusPointBar.gameObject.SetActive(false);
            focusPointBar.gameObject.SetActive(true);
        }

        public void SetRunesCount(int runesToAdd)
        {
            pendingRunesToAdd += runesToAdd;

            if (waitThenAddRunesCoroutine != null)
                StopCoroutine(waitThenAddRunesCoroutine);

            waitThenAddRunesCoroutine = StartCoroutine(WaitThenUpdateRuneCount());


        }

        private IEnumerator WaitThenUpdateRuneCount()
        {
            float timer = runeUpdateCountDelayTimer;
            int runesToAdd = pendingRunesToAdd;
            runesToAddText.text = "+ " + runesToAdd.ToString();
            runesToAddText.enabled = true;

            while (timer > 0)
            {
                timer -= Time.deltaTime;

                if (runesToAdd != pendingRunesToAdd)
                {
                    runesToAdd = pendingRunesToAdd;
                    runesToAddText.text = "+ " + runesToAdd.ToString();
                }

                yield return null;
            }
            runesToAddText.enabled = false;
            pendingRunesToAdd = 0;
            runesCountText.text = PlayerUIManager.Singleton.localPlayer.playerStatsManager.runes.ToString();
            yield return null;
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void SetNewFocusPointValue(int oldValue, int newValue)
        {
            focusPointBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxFocusPointsValue(int maxFocusPoint)
        {
            focusPointBar.SetMaxStat(maxFocusPoint);
        }

        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {

            WeaponItem weapon = WorldItemDatabase.Singleton.GetWeaponByID(weaponID);

            if (weapon == null)
            {
                Debug.Log("Right weapon is null");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.Log("Right Weapon has No Icon");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }



            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            rightWeaponQuickSlotIcon.enabled = true;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {

            WeaponItem weapon = WorldItemDatabase.Singleton.GetWeaponByID(weaponID);

            if (weapon == null)
            {
                Debug.Log("Left weapon is null");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.Log("left Weapon has No Icon");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
            leftWeaponQuickSlotIcon.enabled = true;
        }

        public void SetSpellQuickSlotIcon(int spellID)
        {

            SpellItem spell = WorldItemDatabase.Singleton.GetSpellByID(spellID);

            if (spell == null)
            {
                Debug.Log("spell is null");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }

            if (spell.itemIcon == null)
            {
                Debug.Log("spell has No Icon");
                spellItemQuickSlotIcon.enabled = false;
                spellItemQuickSlotIcon.sprite = null;
                return;
            }

            spellItemQuickSlotIcon.sprite = spell.itemIcon;
            spellItemQuickSlotIcon.enabled = true;
        }

        public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)
        {

            if (quickSlotItem == null)
            {
                Debug.Log("quickSlotItem is null");
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                quickSlotItemCount.enabled = false;
                return;
            }

            if (quickSlotItem.itemIcon == null)
            {
                Debug.Log("quickSlotItem has No Icon");
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                quickSlotItemCount.enabled = false;
                return;
            }

            quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
            quickSlotItemQuickSlotIcon.enabled = true;

            if (quickSlotItem.isConsumable)
            {
                PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
                quickSlotItemCount.text = quickSlotItem.GetCurrentAmount(player).ToString();
                quickSlotItemCount.enabled = true;
            }
            else
            {
                quickSlotItemCount.enabled = false;
            }
        }

        public void ToggleProjectileQuickSlotsVisibility(bool status)
        {
            projectileQuickSlotsGameObject.SetActive(status);
        }

        public void SetMainProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.Log("mainProjectile is null");
                mainProjectileQuickSlotIcon.enabled = false;
                mainProjectileQuickSlotIcon.sprite = null;
                mainProjectileCount.enabled = false;
                return;
            }

            if (projectileItem.itemIcon == null)
            {
                Debug.Log("mainProjectile has No Icon");
                mainProjectileQuickSlotIcon.enabled = false;
                mainProjectileQuickSlotIcon.sprite = null;
                mainProjectileCount.enabled = false;
                return;
            }

            mainProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
            mainProjectileCount.text = projectileItem.currentAmmoAmount.ToString();
            mainProjectileQuickSlotIcon.enabled = true;
            mainProjectileCount.enabled = false;
        }

        public void SetSecondaryProjectileQuickSlotIcon(RangedProjectileItem projectileItem)
        {
            if (projectileItem == null)
            {
                Debug.Log("secondaryProjectile is null");
                secondaryProjectileQuickSlotIcon.enabled = false;
                secondaryProjectileQuickSlotIcon.sprite = null;
                secondaryProjectileCount.enabled = false;
                return;
            }

            if (projectileItem.itemIcon == null)
            {
                Debug.Log("secondaryProjectile has No Icon");
                secondaryProjectileQuickSlotIcon.enabled = false;
                secondaryProjectileQuickSlotIcon.sprite = null;
                secondaryProjectileCount.enabled = false;
                return;
            }

            secondaryProjectileQuickSlotIcon.sprite = projectileItem.itemIcon;
            secondaryProjectileCount.text = projectileItem.currentAmmoAmount.ToString();
            secondaryProjectileQuickSlotIcon.enabled = true;
            secondaryProjectileCount.enabled = false;
        }

    }

}