#nullable enable

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the FPS counter visibility state using a UI Toggle.
/// Persists the user's preference with PlayerPrefs and notifies
/// other components via a static event when the state changes.
/// </summary>
public class FPSToggleManager : MonoBehaviour
{
    private const string ShowFPSKey = "ShowFPS";

    [SerializeField] private Toggle? fpsToggle;
    public static bool IsFPSEnabled => PlayerPrefs.GetInt(ShowFPSKey, 0) == 1;

    public static event System.Action<bool>? OnFPSStateChanged;

    private void Start()
    {
        if (fpsToggle != null)
        {
            fpsToggle.SetIsOnWithoutNotify(IsFPSEnabled);
            fpsToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        OnFPSStateChanged?.Invoke(IsFPSEnabled);
    }

    private void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(ShowFPSKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        OnFPSStateChanged?.Invoke(isOn);

        Debug.Log($"FPS отображение: {(isOn ? "ВКЛ" : "ВЫКЛ")}");
    }

    private void OnDestroy()
    {
        if (fpsToggle != null)
        {
            fpsToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}
