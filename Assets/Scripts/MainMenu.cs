using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu navigation and application control.
/// Provides methods for starting the game, opening settings, and exiting the application.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private const string SettingsSceneName = "1-SettingsScene";
    private const string SampleSceneName = "2-SampleScene";

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(SampleSceneName);
    }

    public void SettingsGame()
    {
        SceneManager.LoadScene(SettingsSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}