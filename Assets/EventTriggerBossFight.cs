using UnityEngine;

namespace TraverserProject
{

    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] int bossID;

        private void OnTriggerEnter(Collider other)
        {
            AIBossCharacterManager boss = WorldAIManager.Singleton.GetBossCharacterByID(bossID);

            if (boss != null)
            {
                boss.WakeBoss();
            }
        }

    }
}