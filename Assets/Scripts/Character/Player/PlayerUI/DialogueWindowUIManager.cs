using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class DialogueWindowUIManager : PlayerUIMenu
    {
        [Header("Dialogue Character")]
        [SerializeField] TextMeshProUGUI aiCharacterNameText;

        [Header("Dialogue Options")]
        [SerializeField] Button talkToCharacterButton; //always enabled
        [SerializeField] Button buyItemsButton;         //only enabled if shopkeeper
        [SerializeField] Button sellItemsButton;        //only enabled if shopkeeper
        [SerializeField] Button blacksmithButton;       //only enabled if blacksmith

        public override void OpenMenu()
        {

            base.OpenMenu();
        }
    }
}