using UnityEngine;

public class NpcSurvivalHealth : MonoBehaviour
{
    [SerializeField] private NpcSurvivalConfigSO _config;

    [Header("Изменяемые текущие показатели")]
    [SerializeField] private float _health;
    [SerializeField] private float _hunger;
    [SerializeField] private float _thirst;

    public NpcSurvivalConfigSO Config => _config;
    
    public float Health
    {
        get { return _health; }
        private set
        {
            _health = Mathf.Clamp(value, 0f, _config.MaxHealth);
            if (_health <= 0f)
            {
                Debug.Log("НПС погиб!");
            }
        }
    }

    public float Hunger
    {
        get { return _hunger; }
        private set { _hunger = Mathf.Clamp(value, 0f, _config.MaxHunger); }
    }

    public float Thirst
    {
        get { return _thirst; }
        private set { _thirst = Mathf.Clamp(value, 0f, _config.MaxThirst); }
    }

    private void Start()
    {
        Health = _config.MaxHealth;
        Hunger = _config.MaxHunger;
        Thirst = _config.MaxThirst;
    }

    //Здоровье
    public void IncreaseHealth(float amount) => Health += amount;
    public void DecreaseHealth(float amount) => Health -= amount;

    //Голод
    public void IncreaseHunger(float amount) => Hunger += amount;
    public void DecreaseHunger(float amount) => Hunger -= amount;

    //Жажда
    public void IncreaseThirst(float amount) => Thirst += amount;
    public void DecreaseThirst(float amount) => Thirst -= amount;
}