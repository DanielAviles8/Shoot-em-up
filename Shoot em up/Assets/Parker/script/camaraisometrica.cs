using UnityEngine;

public class CameraIsometric2D : MonoBehaviour
{
    public Transform target;  // Objetivo a seguir (jugador)

    [Header("Seguimiento")]
    public float smoothSpeed = 5f; // Velocidad de seguimiento
    public Vector3 offset = new Vector3(0, 5, -10); // Posición relativa

    [Header("Rotación")]
    public float rotationSpeed = 100f; // Velocidad de rotación
    private float currentRotationZ = 0f; // Rotación inicial

    [Header("Zoom")]
    public float zoomSpeed = 5f; // Velocidad del zoom
    public float minZoom = 3f;  // Mínimo acercamiento
    public float maxZoom = 15f; // Máximo alejamiento

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 🔄 Manejo de rotación con teclas Q y E (solo en eje Z para 2D)
        if (Input.GetKey(KeyCode.Q))
            currentRotationZ += rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            currentRotationZ -= rotationSpeed * Time.deltaTime;

        // 🔍 Manejo de Zoom con la rueda del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);

        // 📍 Calcular posición deseada y aplicar suavizado
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 📌 Aplicar rotación en 2D (eje Z)
        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
    }
}
