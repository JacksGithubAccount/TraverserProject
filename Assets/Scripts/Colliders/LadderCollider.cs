using UnityEngine;

namespace TraverserProject
{
    public class LadderCollider : MonoBehaviour
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();


        }
    }
}
