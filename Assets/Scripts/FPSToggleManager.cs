using UnityEngine;
using UnityEngine.UI;

public class FPSToggleManager : MonoBehaviour
{
    private const string ShowFPSKey = "ShowFPS";

    [SerializeField] private Toggle fpsToggle;

    // Статическое свойство: доступно из любого скрипта в игре
    public static bool IsFPSEnabled => PlayerPrefs.GetInt(ShowFPSKey, 0) == 1;

    // Событие: уведомляет другие скрипты (например, FPSCounter) об изменении
    public static event System.Action<bool> OnFPSStateChanged;

    private void Start()
    {
        if (fpsToggle != null)
        {
            // 1. Устанавливаем положение переключателя БЕЗ вызова события (чтобы не было зацикливания)
            fpsToggle.SetIsOnWithoutNotify(IsFPSEnabled);

            // 2. Подписываемся на изменение только один раз
            fpsToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        // 3. Сразу применяем состояние (на случай, если другая сцена уже загружена)
        OnFPSStateChanged?.Invoke(IsFPSEnabled);
    }

    private void OnToggleChanged(bool isOn)
    {
        // Сохраняем выбор игрока навсегда (до смены значения)
        PlayerPrefs.SetInt(ShowFPSKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Уведомляем все скрипты, которые "слушают" это событие
        OnFPSStateChanged?.Invoke(isOn);

        Debug.Log($"FPS отображение: {(isOn ? "ВКЛ" : "ВЫКЛ")}");
    }

    private void OnDestroy()
    {
        // Чистим за собой, чтобы избежать ошибок при уничтожении объекта
        if (fpsToggle != null)
        {
            fpsToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}
