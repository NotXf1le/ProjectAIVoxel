using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Точка, вокруг которой камера будет вращаться
    public float rotationSpeed = 5f; // Скорость вращения камеры
    public float zoomSpeed = 10f; // Скорость зума
    public float minZoom = 5f; // Минимальное расстояние до точки
    public float maxZoom = 50f; // Максимальное расстояние до точки
    public float smoothing = 0.1f; // Параметр сглаживания для плавности движения камеры

    private float currentZoom = 10f; // Текущее расстояние
    private float targetRotationX = 0f; // Целевое вращение по оси X
    private float targetRotationY = 0f; // Целевое вращение по оси Y
    private float currentRotationX = 0f; // Текущее вращение по оси X
    private float currentRotationY = 0f; // Текущее вращение по оси Y
    private bool isRotating = false; // Флаг, чтобы определить, когда вращать камеру

    private void Start()
    {
        currentRotationX = transform.rotation.eulerAngles.x;
        currentRotationX = transform.rotation.eulerAngles.y;

    }
    void Update()
    {
        if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl))
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }

        // Вращение камеры вокруг точки при движении мыши
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            targetRotationX -= mouseY * rotationSpeed;
            targetRotationY += mouseX * rotationSpeed;

            // Ограничиваем вращение по оси X (вверх/вниз)
            targetRotationX = Mathf.Clamp(targetRotationX, -15f, 90f);
        }

        // Плавное изменение углов
        currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, smoothing);
        currentRotationY = Mathf.Lerp(currentRotationY, targetRotationY, smoothing);

        // Управление зумом с помощью колесика мыши
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentZoom -= scrollInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        // Плавное изменение зума
        currentZoom = Mathf.Lerp(currentZoom, currentZoom, smoothing);

        // Обновляем позицию камеры с учётом вращения и зума
        UpdateCameraPosition();
    }

    // Метод для обновления позиции камеры с учётом вращения и зума
    void UpdateCameraPosition()
    {
        // Вычисляем направление камеры относительно целевой точки
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 offset = new Vector3(0, 0, -currentZoom);

        // Вычисляем новую позицию камеры
        transform.position = target.position + rotation * offset;

        // Камера всегда смотрит на целевую точку
        transform.LookAt(target);
    }
}