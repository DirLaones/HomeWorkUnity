using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    public enum DistributionType { Uniform, Chain }

    [Header("Настройки создания")]
    [SerializeField] private GameObject cubePrefab; 
    [SerializeField] private int cubeCount = 5;      
    [SerializeField] private DistributionType distribution = DistributionType.Uniform; 
    [SerializeField] private float chainSpacingAngle = 10f; 

    [Header("Настройки движения")]
    [SerializeField] private float radius = 5f;        
    [SerializeField] private float rotationSpeed = 50f; 
    [SerializeField] private bool clockwise = true;     

    [Header("Настройки покачивания (Вверх/Вниз)")]
    [SerializeField] private float waveAmplitude = 0.5f; // Высота покачивания вверх и вниз
    [SerializeField] private float waveFrequency = 2f;    // Скорость самого покачивания
    [SerializeField] private bool useWaveOffset = true;   // true — эффект волны, false — все качаются синхронно

    private GameObject[] spawnedCubes;
    private float[] currentAngles;
    private float waveTimer; // Отдельный таймер для независимой частоты покачивания

    private void Awake()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Пожалуйста, назначьте Cube Prefab в инспекторе!");
            return;
        }

        spawnedCubes = new GameObject[cubeCount];
        currentAngles = new float[cubeCount];

        for (int i = 0; i < cubeCount; i++)
        {
            float initialAngle = 0f;
            if (distribution == DistributionType.Uniform)
            {
                initialAngle = i * (360f / cubeCount);
            }
            else if (distribution == DistributionType.Chain)
            {
                initialAngle = i * chainSpacingAngle;
            }

            currentAngles[i] = initialAngle;

            GameObject cube = Instantiate(cubePrefab, transform);
            spawnedCubes[i] = cube;

            UpdateCubePosition(i);
        }
    }

    private void Update()
    {
        if (spawnedCubes == null) return;

        // Направление вращения по кругу
        float direction = clockwise ? -1f : 1f;

        // Обновляем таймер для покачивания
        waveTimer += Time.deltaTime * waveFrequency;

        for (int i = 0; i < spawnedCubes.Length; i++)
        {
            if (spawnedCubes[i] == null) continue;

            // Изменяем угол вращения по кругу
            currentAngles[i] += direction * rotationSpeed * Time.deltaTime;
            currentAngles[i] %= 360f;

            // Обновляем позицию (включая новую высоту Y)
            UpdateCubePosition(i);
        }
    }

    private void UpdateCubePosition(int index)
    {
        // Вычисляем координаты X и Z на плоскости для вращения по кругу
        float radian = currentAngles[index] * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * radius;
        float z = Mathf.Sin(radian) * radius;

        // Вычисляем высоту Y с помощью синусоиды
        // Если useWaveOffset включен, добавляем смещение в зависимости от индекса кубика (эффект волны)
        float offset = useWaveOffset ? index * 0.5f : 0f;
        float y = Mathf.Sin(waveTimer + offset) * waveAmplitude;

        // Устанавливаем итоговую позицию кубика относительно центрального объекта
        spawnedCubes[index].transform.position = transform.position + new Vector3(x, y, z);
    }
}
