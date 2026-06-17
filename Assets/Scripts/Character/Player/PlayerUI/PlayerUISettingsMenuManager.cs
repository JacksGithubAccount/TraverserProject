using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUISettingsMenuManager : PlayerUIMenu
    {
        [Header("Display Settings")]
        [SerializeField] GameObject displayWindow;
        public TMP_Dropdown resolutionDropDown;

        [Header("Sound Settings")]
        [SerializeField] GameObject soundWindow;
        public Slider masterSlider;
        public Slider bgmSlider;
        public Slider sfxSlider;

        [Header("Remap Settings")]
        [SerializeField] GameObject remapWindow;



        public override void OpenMenu()
        {
            base.OpenMenu();

            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            LoadDisplayWindow();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
        }

        public void DisableAllSettingsWindow()
        {
            displayWindow.SetActive(false);
            soundWindow.SetActive(false);
            remapWindow.SetActive(false);
        }

        public void LoadDisplayWindow()
        {
            DisableAllSettingsWindow();
            displayWindow.SetActive(true);
        }
        public void LoadSoundWindow()
        {
            DisableAllSettingsWindow();
            soundWindow.SetActive(true);
        }
        public void LoadRemapWindow()
        {
            DisableAllSettingsWindow();
            remapWindow.SetActive(true);
        }
        public void ToggleFullscreen(bool toggle)
        {
            Screen.fullScreen = toggle;
        }

        // Set explicitly with a custom resolution
        public void SetTrueFullscreen()
        {
            // Sets resolution and enforces exclusive/borderless fullscreen
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
        }
        public void SetResolution()
        {
            int width = 0;
            int height = 0;

            if (resolutionDropDown.value == 0)
            {
                width = 3840;
                height = 2160;
            }
            else if (resolutionDropDown.value == 1)
            {
                width = 1920;
                height = 1080;
            }
            else if (resolutionDropDown.value == 2)
            {
                width = 1280;
                height = 720;
            }
            else if (resolutionDropDown.value == 3)
            {
                width = 720;
                height = 576;
            }
            else if (resolutionDropDown.value == 4)
            {
                width = 720;
                height = 480;
            }
            Screen.SetResolution(width, height, Screen.fullScreenMode);
        }
        public void SetMasterVolume()
        {
            AudioListener.volume = masterSlider.value;
        }

        public void SetBGMVolume()
        {
            WorldSoundFXManager.Singleton.bgmAudioSource.volume = bgmSlider.value;
        }
        public void SetSFXVolume()
        {
            WorldSoundFXManager.Singleton.masterAudioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
        }
    }
}
