using UnityEngine;

[CreateAssetMenu(fileName = "NpcSurvivalConfig", menuName = "Survival/NPC Config")]
public class NpcSurvivalConfigSO : ScriptableObject
{
    [Header("Неизменяемые максимальные значения")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _maxHunger = 100f;
    [SerializeField] private float _maxThirst = 100f;
    
    public float MaxHealth
    {
        get { return _maxHealth; }
    }

    public float MaxHunger
    {
        get { return _maxHunger; }
    }

    public float MaxThirst
    {
        get { return _maxThirst; }
    }
}