using UnityEngine;
using TMPro;

/// <summary>
/// Скрипт отображает текущий FPS (кадры в секунду) на экране.
/// Работает в связке с FPSToggleManager через систему событий.
/// Автоматически создает UI-текст, если он не назначен вручную.
/// </summary>
public class FPSCounter : MonoBehaviour
{
    [Header("Настройки отображения")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float updateInterval = 0.5f; // Как часто обновлять текст (в секундах)

    // Приватные переменные для расчёта
    private float elapsedTime;
    private int frameCount;
    private bool isVisible;

    // ===== ЖИЗНЕННЫЙ ЦИКЛ =====

    private void Start()
    {
        // Если текст не назначен в Inspector, создаём его автоматически
        if (fpsText == null)
        {
            CreateFPSText();
        }

        if (fpsText != null)
        {
            ConfigureText();
        }

        // Применяем начальное состояние сразу при старте
        UpdateVisibility(FPSToggleManager.IsFPSEnabled);
    }

    private void OnEnable()
    {
        // Подписываемся на событие изменения состояния при активации объекта
        FPSToggleManager.OnFPSStateChanged += UpdateVisibility;
    }

    private void OnDisable()
    {
        // Обязательно отписываемся при деактивации, чтобы избежать утечек памяти
        FPSToggleManager.OnFPSStateChanged -= UpdateVisibility;
    }

    // ===== ОБРАБОТЧИК СОБЫТИЯ =====

    /// <summary>
    /// Вызывается автоматически, когда игрок меняет состояние Toggle
    /// </summary>
    private void UpdateVisibility(bool isEnabled)
    {
        isVisible = isEnabled;

        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(isEnabled);

            if (isEnabled)
            {
                fpsText.text = "FPS: --";
            }
        }

        // Сбрасываем накопленные данные, чтобы после включения не показывалось старое значение.
        elapsedTime = 0f;
        frameCount = 0;
    }

    // ===== РАСЧЁТ FPS =====

    private void Update()
    {
        if (!isVisible)
        {
            return;
        }

        elapsedTime += Time.unscaledDeltaTime;
        frameCount++;

        // Обновляем текст только когда прошел заданный интервал
        if (elapsedTime >= Mathf.Max(0.1f, updateInterval))
        {
            int fps = Mathf.RoundToInt(frameCount / elapsedTime);

            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps}";
            }

            // Сброс счётчиков
            frameCount = 0;
            elapsedTime = 0f;
        }
    }

    // ===== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====

    private void CreateFPSText()
    {
        // Ищем первый активный Canvas в сцене
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogWarning("Canvas не найден в сцене. FPS Counter не будет создан.");
            return;
        }

        // Создаём новый объект
        GameObject fpsObject = new GameObject("FPSCounter_Object");
        fpsObject.transform.SetParent(canvas.transform, false);

        // Добавляем компоненты
        fpsText = fpsObject.AddComponent<TextMeshProUGUI>();
    }

    private void ConfigureText()
    {
        fpsText.color = textColor;
        fpsText.fontSize = 36;
        fpsText.alignment = TextAlignmentOptions.TopRight;
        fpsText.raycastTarget = false;
        fpsText.text = "FPS: --";

        // Закрепляем счётчик в правом верхнем углу с отступом от краёв экрана.
        RectTransform rectTransform = fpsText.rectTransform;
        rectTransform.anchorMin = Vector2.one;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = Vector2.one;
        rectTransform.anchoredPosition = new Vector2(-20f, -20f);
        rectTransform.sizeDelta = new Vector2(240f, 60f);
    }
}
