using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            //characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
        protected virtual void Update()
        {
            /*if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;

            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);
            }*/
        }
    }
}