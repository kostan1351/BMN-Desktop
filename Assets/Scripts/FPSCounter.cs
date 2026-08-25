
#nullable enable

using UnityEngine;
using TMPro;

/// <summary>
/// Manages an FPS counter displayed with TextMeshPro.
/// Updates at a configurable interval, supports visibility toggling,
/// and auto-creates a UI element if no reference is provided.
/// </summary>

public class FPSCounter : MonoBehaviour
{
    [Header("Настройки отображения")]
    [SerializeField] private TextMeshProUGUI? fpsText;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float updateInterval = 0.5f;

    private float elapsedTime;
    private int frameCount;
    private bool isVisible;

    private void Start()
    {
        if (fpsText == null)
        {
            CreateFPSText();
        }

        if (fpsText != null)
        {
            ConfigureText(fpsText);
        }

        UpdateVisibility(FPSToggleManager.IsFPSEnabled);
    }

    private void OnEnable()
    {
        FPSToggleManager.OnFPSStateChanged += UpdateVisibility;
    }

    private void OnDisable()
    {
        FPSToggleManager.OnFPSStateChanged -= UpdateVisibility;
    }
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

        elapsedTime = 0f;
        frameCount = 0;
    }

    private void Update()
    {
        if (!isVisible || fpsText == null)
        {
            return;
        }

        elapsedTime += Time.unscaledDeltaTime;
        frameCount++;

        if (elapsedTime >= Mathf.Max(0.1f, updateInterval))
        {
            int fps = Mathf.RoundToInt(frameCount / elapsedTime);

            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps}";
            }

            frameCount = 0;
            elapsedTime = 0f;
        }
    }

    private void CreateFPSText()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        GameObject fpsObject = new GameObject("FPSCounter_Object");
        fpsObject.transform.SetParent(canvas.transform, false);

        fpsText = fpsObject.AddComponent<TextMeshProUGUI>();
    }

    private void ConfigureText(TextMeshProUGUI text)
    {
        text.color = textColor;
        text.fontSize = 36;
        text.alignment = TextAlignmentOptions.TopRight;
        text.raycastTarget = false;
        text.text = "FPS: --";

        RectTransform rectTransform = text.rectTransform;
        rectTransform.anchorMin = Vector2.one;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = Vector2.one;
        rectTransform.anchoredPosition = new Vector2(-20f, -20f);
        rectTransform.sizeDelta = new Vector2(240f, 60f);
    }
}
