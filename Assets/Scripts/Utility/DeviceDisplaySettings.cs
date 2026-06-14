using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public struct CustomInputContextIcon
    {
        public string customInputContextString;
        public Sprite customInputContextIcon;
    }

    [CreateAssetMenu(fileName = "Device Display Settings", menuName = "Scriptable Objects/Device Display Settings", order = 1)]
    public class DeviceDisplaySettings : ScriptableObject
    {
        [Header("Display Name")]
        public string deviceDisplayName;

        [Header("Display Color")]
        public Color deviceDisplayColor;

        [Header("Icon Settings")]
        public bool deviceHasContextIcons;

        [Header("Icons - Action Buttons")]
        public Sprite buttonNorthIcon;
        public Sprite buttonSouthIcon;
        public Sprite buttonWestIcon;
        public Sprite buttonEastIcon;

        [Header("Icons - Triggers")]
        public Sprite triggerRightFrontIcon;
        public Sprite triggerRightBackIcon;
        public Sprite triggerLeftFrontIcon;
        public Sprite triggerLeftBackIcon;

        [Header("Icons - Custom Contexts")]
        public List<CustomInputContextIcon> customContextIcons = new List<CustomInputContextIcon>();

    }
}