// Подключение пространств имён для работы с системой и сценами
// System.Collections - для работы с коллекциями (списки, очереди, пр.)
// System.Collections.Generic - версия с обобщёнными типами (как templates в C++)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Для загрузки сцен

// ===== ГЛАВНОЕ МЕНЮ =====
// Этот класс управляет главным меню - обрабатывает клики на кнопки
// public класс означает, что его можно использовать в других скриптах

public class MainMenu : MonoBehaviour
{
    private const string SettingsSceneName = "1-SettingsScene";
    private const string SampleSceneName = "2-SampleScene";

    // ===== МЕТОДЫ =====

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Debug.Log($"Курсор: {Cursor.lockState}");
        Debug.Log($"Видимость курсора: {Cursor.visible}");
    }
    
    // ===== МЕТОД STARTGAME =====
    // public - доступен из Inspector для привязки к кнопкам UI
    // void - не возвращает значение (процедура, как в Pascal)
    // Этот метод вызывается при нажатии кнопки "Start Game" в меню
    public void StartGame()
    {
        // SceneManager.LoadScene() - загружает сцену по индексу или имени
        // Загружаем игровую сцену по имени, поэтому её порядок в Build Settings не важен
        // Как в Python: scene_manager.load_scene("2-SampleScene")
        SceneManager.LoadScene(SampleSceneName);
    }

    // ===== МЕТОД SETTINGSGAME =====
    // Этот метод вызывается при нажатии кнопки "Settings" в меню
    public void SettingsGame()
    {
        // Загружаем сцену настроек по имени
        SceneManager.LoadScene(SettingsSceneName);
    }

    // ===== МЕТОД EXITGAME =====
    // Этот метод вызывается при нажатии кнопки "Exit" в меню
    public void ExitGame()
    {
        // Application.Quit() - закрывает приложение/игру
        // Работает только в собранной (compiled) игре, не в редакторе Unity
        Application.Quit();

        // ===== УСЛОВНАЯ КОМПИЛЯЦИЯ =====
        // #if UNITY_EDITOR - условие: если код выполняется в редакторе Unity
        // Это означает: "если мы в режиме разработки/редактора"
        
        // Для тестирования в редакторе Unity добавляем альтернативный способ выхода
        // EditorApplication.isPlaying = false - останавливает режим Play в редакторе
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // #endif - конец условного блока
    }
}

// ===== СПРАВКА ПО СИНТАКСИСУ C# И UNITY =====
// 
// 1. public / void / private - модификаторы доступа и типы возвращаемых значений
//    public - видимо везде
//    private - видимо только внутри класса
//    void - функция ничего не возвращает
//
// 2. MonoBehaviour - базовый класс Unity
//    Даёт доступу к методам: Start(), Update(), OnDestroy() и т.д.
//
// 3. SceneManager - статический класс для управления сценами
//    LoadScene(index) - загружает сцену по индексу
//    LoadScene(name) - загружает сцену по имени
//
// 4. Application - статический класс для управления приложением
//    Quit() - закрывает приложение
//
// 5. #if / #endif - условная компиляция (как #ifdef в C++)
//    Код внутри выполняется только при указанных условиях
//    UNITY_EDITOR - переменная, которая истина в редакторе Unity
//
// 6. Методы в Inspector:
//    public void - методы автоматически видны в Inspector Unity
//    Их можно привязать к кнопкам UI через события (Events)
