using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Settings menu controller. Provides a method to return to the main menu.
/// </summary>

public class SettingsMenu : MonoBehaviour
{
    private const string MainMenuSceneName = "0-MainScence";
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
