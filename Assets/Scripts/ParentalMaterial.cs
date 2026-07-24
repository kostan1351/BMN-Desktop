using UnityEngine;

public class ApplyMaterialToHierarchy : MonoBehaviour
{
    [SerializeField] private Material materialToApply; // Сюда перетащите ваш серебряный материал

    [ContextMenu("Apply Material To Children")]
    private void ApplyMaterial()
    {
        if (materialToApply == null)
        {
            Debug.LogWarning("Material is not assigned!", this);
            return;
        }

        // Находим все компоненты Renderer на родителе И на ВСЕХ его детях
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No Renderers found in the hierarchy!", this);
            return;
        }

        // Проходим по каждому найденному рендереру и назначаем материал
        foreach (Renderer rend in renderers)
        {
            rend.material = materialToApply;
        }

        Debug.Log($"Material applied to {renderers.Length} renderer(s).");
    }
}