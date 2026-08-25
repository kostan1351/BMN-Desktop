#nullable enable

using UnityEngine;

/// <summary>
/// Applies a specified material to all child Renderers in the hierarchy.
/// Can be triggered manually via the context menu in the Inspector.
/// </summary>
public class ApplyMaterialToHierarchy : MonoBehaviour
{
    [SerializeField] private Material? materialToApply;

    [ContextMenu("Apply Material To Children")]
    private void ApplyMaterial()
    {
        if (materialToApply == null)
        {
            Debug.LogWarning("Material is not assigned!", this);
            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No Renderers found in the hierarchy!", this);
            return;
        }

        foreach (Renderer rend in renderers)
        {
            rend.material = materialToApply;
        }

        Debug.Log($"Material applied to {renderers.Length} renderer(s).");
    }
}