using UnityEngine;

public class GeckoController : MonoBehaviour
{
    // The target we are going to track
    [SerializeField] Transform target;
    // A reference to the gecko's neck
    [SerializeField] Transform headBone;

    // We will put all our animation code in LateUpdate.
    // This allows other systems to update the environment first,
    // allowing the animation system to adapt to it before the frame is drawn.
    void LateUpdate()
    {
        // Bone manipulation code goes here!
        Vector3 towardObjectFromHead = target.position - headBone.position;
        headBone.rotation = Quaternion.LookRotation(towardObjectFromHead, transform.up);
    }
}
