using UnityEngine;
using UnityEngine.UI;

public class FPSToggleManager : MonoBehaviour
{
    [SerializeField] private Toggle fpsToggle;

    // Статическое свойство: доступно из любого скрипта в игре
    public static bool IsFPSEnabled { get; private set; }

    // Событие: уведомляет другие скрипты (например, FPSCounter) об изменении
    public static event System.Action<bool> OnFPSStateChanged;

    private void Start()
    {
        // 1. Загружаем сохраненное состояние (0 = выкл, 1 = вкл). По умолчанию выкл.
        IsFPSEnabled = PlayerPrefs.GetInt("ShowFPS", 0) == 1;

        if (fpsToggle != null)
        {
            // 2. Устанавливаем положение переключателя БЕЗ вызова события (чтобы не было зацикливания)
            fpsToggle.SetIsOnWithoutNotify(IsFPSEnabled);

            // 3. Подписываемся на изменение только один раз
            fpsToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        // 4. Сразу применяем состояние (на случай, если другая сцена уже загружена)
        OnFPSStateChanged?.Invoke(IsFPSEnabled);
    }

    private void OnToggleChanged(bool isOn)
    {
        IsFPSEnabled = isOn;

        // Сохраняем выбор игрока навсегда (до смены значения)
        PlayerPrefs.SetInt("ShowFPS", isOn ? 1 : 0);
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