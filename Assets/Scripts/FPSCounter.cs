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
    private float deltaTime = 0f;
    private float updateTimer = 0f;
    private int frameCount = 0;

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
            fpsText.color = textColor;
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
        if (fpsText != null)
        {
            fpsText.gameObject.SetActive(isEnabled);
        }

        // КЛЮЧЕВАЯ ОПТИМИЗАЦИЯ: 
        // Если FPS не нужен, мы отключаем сам этот скрипт.
        // Метод Update() перестанет вызываться, экономя ресурсы процессора.
        this.enabled = isEnabled;
    }

    // ===== РАСЧЁТ FPS =====

    private void Update()
    {
        // Этот код выполняется ТОЛЬКО если this.enabled == true

        deltaTime += Time.unscaledDeltaTime;
        frameCount++;
        updateTimer += Time.unscaledDeltaTime;

        // Обновляем текст только когда прошел заданный интервал
        if (updateTimer >= updateInterval)
        {
            float fps = frameCount / deltaTime;

            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps:F1}";
            }

            // Сброс счётчиков
            frameCount = 0;
            deltaTime = 0f;
            updateTimer = 0f;
        }
    }

    // ===== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====

    private void CreateFPSText()
    {
        // Ищем любой активный Canvas в сцене
        Canvas canvas = FindObjectOfType<Canvas>();

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
        RectTransform rectTransform = fpsObject.GetComponent<RectTransform>();

        // Настраиваем внешний вид
        fpsText.fontSize = 36;
        fpsText.alignment = TextAlignmentOptions.BottomLeft;
        fpsText.text = "FPS: 0";
        fpsText.color = textColor;

        // Настраиваем позицию и размер (левый нижний угол с отступом)
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(20, 20); // Отступ 20px от края
        rectTransform.sizeDelta = new Vector2(200, 50);
    }
}