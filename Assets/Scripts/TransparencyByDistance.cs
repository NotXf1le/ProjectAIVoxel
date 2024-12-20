using UnityEngine;

public class TransparencyByDistance : MonoBehaviour
{
    public Renderer objectRenderer; // Рендер объекта, прозрачность которого будет изменяться
    public float minDistance = 5f; // Минимальное расстояние для максимальной прозрачности
    public float maxDistance = 20f; // Максимальное расстояние для минимальной прозрачности
    public float maxTransparency = 1f; // Максимальная прозрачность (0 - полностью прозрачный, 1 - полностью непрозрачный)
    public float minTransparency = 0f; // Минимальная прозрачность (0 - полностью прозрачный, 1 - полностью непрозрачный)

    private Material material; // Материал, с которым будем работать

    void Start()
    {
        // Получаем материал объекта
        if (objectRenderer != null)
        {
            material = objectRenderer.material;
        }
    }

    void Update()
    {
        if (material != null && Camera.main != null)
        {
            // Рассчитываем расстояние между камерой и объектом
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

            // Рассчитываем прозрачность на основе расстояния
            float transparency = Mathf.InverseLerp(minDistance, maxDistance, distance);

            // Применяем clamp для получения прозрачности в допустимом диапазоне
            transparency = Mathf.Clamp(transparency, minTransparency, maxTransparency);

            // Изменяем прозрачность материала
            SetTransparency(transparency);
        }
    }

    // Метод для установки прозрачности материала
    void SetTransparency(float transparency)
    {
        // Убедимся, что материал использует шейдер с альфа-каналом
        if (material.HasProperty("_Color"))
        {
            Color color = material.color;
            color.a = transparency; // Устанавливаем альфа-канал
            material.color = color;
        }
    }
}
