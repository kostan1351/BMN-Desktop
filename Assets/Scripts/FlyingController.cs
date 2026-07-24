// Подключаем необходимые пространства имен (namespace)
// Как в Python: from UnityEngine import *; как в C++: #include <UnityEngine>
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Контроллер для управления полётом объекта в 3D пространстве.
/// Обрабатывает ввод с клавиатуры и мышки, двигает объект и вращает камеру.
/// </summary>

// Ключевое слово public перед классом означает, что класс доступен из других скриптов
// Наследование от MonoBehaviour (: MonoBehaviour) даёт этому классу доступ к Unity функциям
public class FlyingController : MonoBehaviour
{
    // ===== ПУБЛИЧНЫЕ ПЕРЕМЕННЫЕ =====
    // public - переменные видны в Inspector Unity и доступны из других классов
    // float - тип данных для дробных чисел (как float в C++, как float в Python)
    // = 10f - инициализация (f в конце означает, что это float, а не double)
    
    public float flySpeed = 10f;          // Базовая скорость полёта
    public float speedMultiplier = 2f;    // Множитель скорости при нажатии Shift
    public float mouseSensitivity = 2f;   // Чувствительность мышки при вращении камеры

    // ===== ПРИВАТНЫЕ ПЕРЕМЕННЫЕ =====
    // private - переменные видны только внутри этого класса (как static в некоторых языках)
    private Rigidbody rb;                 // Компонент для физики (движение объекта)
    private float verticalRotation = 0;   // Текущий угол вращения камеры по оси Y

    // ===== МЕТОД START =====
    // Вызывается один раз при запуске сцены (как __init__ в Python конструкторе)
    // void - метод не возвращает никакого значения (процедура в Pascal)
    void Start()
    {
        // GetComponent<Rigidbody>() - ищет компонент Rigidbody на этом объекте
        // Как получение атрибута объекта. rb теперь ссылается на компонент физики.
        rb = GetComponent<Rigidbody>();
        
        // Заблокировать и спрятать курсор мышки (для FPS вида от первого лица)
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log($"Блокировка курсора в SampleScene {Cursor.lockState}");
        Debug.Log($"Видимость курсора в SampleScene { Cursor.visible}");

        
        // Отключаем гравитацию (чтобы можно было летать в любом направлении)
        rb.useGravity = false;
        
        // Кинематический режим - объект не взаимодействует с другими телами физики
        // false означает, что объект движется нашим скриптом, а не физикой
        rb.isKinematic = false;
    }

    // ===== МЕТОД UPDATE =====
    // Вызывается каждый кадр (как while loop в игровом цикле)
    // Обрабатывает ввод игрока и обновляет положение
    void Update()
    {
        // Проверяем, нажата ли клавиша Escape
        // if - условный оператор (как в Python, Pascal, C++)
        if (Input.GetKey(KeyCode.Q))
        {
            // Загружаем сцену с индексом 0(главное меню)
            SceneManager.LoadScene(0);
            return; // return прекращает выполнение метода и выходит из функции
        }

        // ===== ОБРАБОТКА МЫШКИ =====
        // Input.GetAxis() - получает числовое значение входа (-1 до 1)
        // "Mouse X" и "Mouse Y" - названия осей, определённые в Unity
        // Как в Python: mouseX = input.getAxis("Mouse X") * mouseSensitivity
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // transform - это компонент позиции и ориентации объекта
        // Rotate() - вращает объект. Vector3.up означает ось Y (вверх)
        // Вращаем тело по оси Y (горизонтально) по движению мышки
        transform.Rotate(Vector3.up * mouseX);

        // -= означает "вычти из переменной" (как в Python, C++)
        // verticalRotation -= mouseY эквивалентно verticalRotation = verticalRotation - mouseY
        verticalRotation -= mouseY;
        
        // Clamp() - ограничивает значение между минимумом и максимумом
        // Это предотвращает переворот камеры (от -90 до +90 градусов)
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        
        // Camera.main - ссылка на главную камеру
        // localRotation устанавливает вращение относительно родителя
        // Quaternion.Euler() - создаёт вращение из углов Эйлера
        // (вертикально, горизонтально, боком)
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // ===== ОБРАБОТКА ДВИЖЕНИЯ =====
        // Input.GetAxis("Horizontal") - получает ввод A/D или стрелок влево/вправо
        // Возвращает от -1 (левая/назад) до 1 (правая/вперёд)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float moveZ = 0;  // Z - для вертикального движения (вверх/вниз)

        // Проверяем специальные клавиши для вертикального движения
        // Input.GetKey() - проверяет, нажата ли клавиша прямо сейчас
        if (Input.GetKey(KeyCode.Space))    // Space - летаем вверх
            moveZ = 1;
        if (Input.GetKey(KeyCode.LeftControl)) // Control - летаем вниз
            moveZ = -1;

        // Если нет никакого движения, останавливаем объект
        // && означает логическое "И" (как in Python: "and")
        // == проверяет равенство
        if (moveX == 0 && moveY == 0 && moveZ == 0)
        {
            // Vector3.zero - это (0, 0, 0), то есть нет движения
            // linearVelocity - скорость движения объекта (в новых версиях Unity)
            rb.linearVelocity = Vector3.zero;
            return; // Прекращаем метод, так как больше нечего делать
        }

        // ===== РАСЧЁТ НАПРАВЛЕНИЯ ДВИЖЕНИЯ =====
        // transform.right - локальная ось X (вправо относительно объекта)
        // transform.forward - локальная ось Z (вперёд относительно объекта)
        // transform.up - локальная ось Y (вверх)
        // Это позволяет двигаться относительно того, куда смотрит камера
        // Vector3 - структура из трёх чисел (X, Y, Z)
        Vector3 move = transform.right * moveX + transform.forward * moveY + transform.up * moveZ;

        // ===== РАСЧЁТ СКОРОСТИ С МНОЖИТЕЛЕМ =====
        float currentSpeed = flySpeed;  // Начинаем с базовой скоростью
        
        // Если нажимаем Shift, ускоряемся
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // *= означает "умножить и присвоить" (как в Python и C++)
            // currentSpeed *= speedMultiplier эквивалентно currentSpeed = currentSpeed * speedMultiplier
            currentSpeed = flySpeed * speedMultiplier;
        }

        // ===== ПРИМЕНЕНИЕ ДВИЖЕНИЯ =====
        // move.normalized - нормализирует вектор (делает его длину = 1)
        // Это нужно чтобы диагональное движение не было быстрее
        // Затем умножаем на скорость и устанавливаем скорость объекта
         rb.linearVelocity = move.normalized * currentSpeed;
     }
}

// ===== СПРАВКА ПО СИНТАКСИСУ C# =====
// 1. using - подключение пространств имён (как import в Python, #include в C++)
// 2. public class - создание класса, видимого из других модулей
// 3. MonoBehaviour - базовый класс Unity, даёт доступ к Start(), Update() и т.д.
// 4. public / private - модификаторы доступа (видимость)
// 5. void - тип возвращаемого значения "ничего" (процедура в Pascal)
// 6. { } - блоки кода (как в Python используется отступ, здесь скобки)
// 7. = - присваивание
// 8. * - умножение (как в Python)
// 9. += / -= / *= - операторы присваивания с операцией
// 10. f - суффикс для float литералов (10f это float, 10 это int)
// 11. GetComponent<Type>() - получает компонент типа Type (generic тип в <> скобках)
// 12. Input.GetAxis() / Input.GetKey() - функции для обработки ввода
