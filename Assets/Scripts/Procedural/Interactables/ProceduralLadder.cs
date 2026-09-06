using UnityEngine;

[ExecuteInEditMode] // Ensures this script runs in the Editor without clicking Play
[RequireComponent(typeof(BoxCollider))]
public class ProceduralLadder : MonoBehaviour
{
    [Header("Ladder Dimensions")]
    [Min(0.5f)] public float height = 3.0f;
    [Min(0.2f)] public float width = 0.6f;

    [Header("Rung Settings")]
    public GameObject rungPrefab;
    [Min(0.1f)] public float rungSpacing = 0.35f;

    [Header("Side Rails (Optional)")]
    public GameObject leftRailPrefab;
    public GameObject rightRailPrefab;

    private BoxCollider boxCollider;

    private void OnValidate()
    {
        // OnValidate runs whenever a value changes in the inspector
        // Use delayCall in Edit mode to bypass Unity's OnValidate destruction restriction
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.delayCall += ExecuteGeneration;
            return;
        }
#endif
        GenerateLadder();
    }

    private void ExecuteGeneration()
    {
#if UNITY_EDITOR
        // Unregister to avoid infinite loops or multi-calls
        UnityEditor.EditorApplication.delayCall -= ExecuteGeneration;
        if (this == null) return;
#endif
        GenerateLadder();
    }

    public void GenerateLadder()
    {
        // 1. Clear out old procedurally generated parts
        ClearGeneratedObjects();

        if (rungPrefab == null) return;

        // 2. Setup the Box Collider trigger zone automatically
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(width, height, 0.5f);
        boxCollider.center = new Vector3(0, height / 2f, 0);

        // 3. Generate Rungs
        int totalRungs = Mathf.FloorToInt(height / rungSpacing);
        for (int i = 0; i <= totalRungs; i++)
        {
            float currentHeight = i * rungSpacing;
            // Prevent placing a rung past the total height of the ladder
            if (currentHeight > height) break;

            GameObject rung = Instantiate(rungPrefab, transform);
            rung.transform.localPosition = new Vector3(0, currentHeight, 0);
            rung.transform.localScale = new Vector3(rung.transform.localScale.x, rung.transform.localScale.y, rung.transform.localScale.z);

            // 4. Generate Rails (Optional)
            GenerateRail(leftRailPrefab, -width / 2f, currentHeight);
            GenerateRail(rightRailPrefab, width / 2f, currentHeight);
        }

        
    }

    private void GenerateRail(GameObject railPrefab, float xPosition, float currentHeight)
    {
        if (railPrefab == null) return;

        GameObject rail = Instantiate(railPrefab, transform);
        rail.transform.localPosition = new Vector3(xPosition, currentHeight, 0);
        rail.transform.localScale = new Vector3(rail.transform.localScale.x, rail.transform.localScale.y, rail.transform.localScale.z);
    }

    private void ClearGeneratedObjects()
    {
        // Safely destroy all child objects in both Edit mode and Play mode
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }
    }
}