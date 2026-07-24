using UnityEngine;

public class FreeCamera : MonoBehaviour {
    public float speed = 10f;
    public float mouseSensitivity = 2f;
    
    void Start() {
        // ПРИНУДИТЕЛЬНОЙ ЗАХВАТ КУРСОРА
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update() {
        // ЕСЛИ КУРСОР НЕ ЗАХВАЧЕН — ЗАХВАТИ
        if (Cursor.lockState != CursorLockMode.Locked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        // WASD
        Vector3 move = Vector3.zero;
        move += Input.GetKey(KeyCode.W) ? transform.forward : Vector3.zero;
        move += Input.GetKey(KeyCode.S) ? -transform.forward : Vector3.zero;
        move += Input.GetKey(KeyCode.A) ? -transform.right : Vector3.zero;
        move += Input.GetKey(KeyCode.D) ? transform.right : Vector3.zero;
        
        transform.Translate(move.normalized * speed * Time.deltaTime, Space.World);
        
        // МЫШЬ
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(-mouseY, mouseX, 0);
    }
}
