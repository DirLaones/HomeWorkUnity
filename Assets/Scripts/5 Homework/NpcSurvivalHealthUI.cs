using UnityEngine;
using UnityEngine.UI;

public class NpcSurvivalHealthUI : MonoBehaviour
{
    [SerializeField] private NpcSurvivalHealth _npc;
    [SerializeField] private float _changeStep = 10f;

    [Header("Шкалы UI")]
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;

    private void Start()
    {
        _healthSlider.maxValue = _npc.Config.MaxHealth;
        _hungerSlider.maxValue = _npc.Config.MaxHunger;
        _thirstSlider.maxValue = _npc.Config.MaxThirst;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _healthSlider.value = _npc.Health;
        _hungerSlider.value = _npc.Hunger;
        _thirstSlider.value = _npc.Thirst;
    }
    
    public void AddHealth() { _npc.IncreaseHealth(_changeStep); UpdateUI(); }
    public void SubHealth() { _npc.DecreaseHealth(_changeStep); UpdateUI(); }

    public void AddHunger() { _npc.IncreaseHunger(_changeStep); UpdateUI(); }
    public void SubHunger() { _npc.DecreaseHunger(_changeStep); UpdateUI(); }

    public void AddThirst() { _npc.IncreaseThirst(_changeStep); UpdateUI(); }
    public void SubThirst() { _npc.DecreaseThirst(_changeStep); UpdateUI(); }
}