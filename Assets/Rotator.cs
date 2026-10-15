using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    public enum DistributionType { Orbit, Chain }

    [Header("Настройки создания")]
    [SerializeField] private GameObject cubePrefab; 
    [SerializeField] private uint cubeCount = 5;      
    [SerializeField] private DistributionType distribution = DistributionType.Orbit; 
    [SerializeField] private float chainSpacingAngle = 10f; 

    [Header("Настройки движения")]
    [SerializeField] private float radius = 5f;        
    [SerializeField] private float rotationSpeed = 50f; 
    [SerializeField] private bool clockwise = true;     

    [Header("Настройки покачивания")]
    [SerializeField] private float waveAmplitude = 0.5f;
    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private bool useWaveOffset = true;
    
    private GameObject[] spawnedCubes;
    private float[] currentAngles;
    private float waveTimer;

    private void Awake()
    {

        spawnedCubes = new GameObject[cubeCount];
        currentAngles = new float[cubeCount];

        for (int i = 0; i < cubeCount; i++)
        {
            float initialAngle = 0f;
            if (distribution == DistributionType.Orbit)
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
        
        float direction = clockwise ? -1f : 1f;
        
        waveTimer += Time.deltaTime * waveFrequency;

        for (int i = 0; i < spawnedCubes.Length; i++)
        {
            if (spawnedCubes[i] == null) continue;
            
            currentAngles[i] += direction * rotationSpeed * Time.deltaTime;
            currentAngles[i] %= 360f;
            
            UpdateCubePosition(i);
        }
    }

    private void UpdateCubePosition(int index)
    {
        float radian = currentAngles[index] * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian) * radius;
        float z = Mathf.Sin(radian) * radius;
        
        float offset = useWaveOffset ? index * 0.5f : 0f;
        float y = Mathf.Sin(waveTimer + offset) * waveAmplitude;
        
        spawnedCubes[index].transform.position = transform.position + new Vector3(x, y, z);
    }
}
