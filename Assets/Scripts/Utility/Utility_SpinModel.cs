using UnityEngine;

public class Utility_SpinModel : MonoBehaviour
{
    [SerializeField] private float xRotation = 0;
    [SerializeField] private float yRotation = 0;
    [SerializeField] private float zRotation = 0;

    private void Update()
    {
        gameObject.transform.eulerAngles += new Vector3(xRotation, yRotation, zRotation);
    }
}
