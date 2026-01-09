using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class DoorInteractable : Interactable
    {
        [Header("Network Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> doorIsOpening = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> doorIsClosing = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] float networkPositionSmoothTime = 0.1f;
    }
}
