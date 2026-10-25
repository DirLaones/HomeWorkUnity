using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceSpawner : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private int diceCount = 5;
    [SerializeField] private float spawnSpacing = 0;
    
    private InputAction rollAction = new InputAction("Roll", InputActionType.Button, "<Keyboard>/space");

    private readonly List<Dice> pool = new List<Dice>();
    private readonly List<Dice> activeDice = new List<Dice>();

    public IReadOnlyList<Dice> ActiveDice => activeDice;
    public int DiceCount
    {
        get => diceCount;
        set => diceCount = Mathf.Max(1, value);
    }
    public event Action OnRoll;

    private void OnEnable()
    {
        rollAction.performed += HandleRollInput;
        rollAction.Enable();
    }

    private void OnDisable()
    {
        rollAction.performed -= HandleRollInput;
        rollAction.Disable();
    }

    private void HandleRollInput(InputAction.CallbackContext context)
    {
        RollDice(diceCount);
    }

    public void RollDice()
    {
        RollDice(diceCount);
    }

    public void RollDice(int count)
    {
        diceCount = Mathf.Max(1, count);

        foreach (var dice in activeDice)
        {
            dice.HideOffMap();
        }
        activeDice.Clear();
        
        while (pool.Count < diceCount)
        {
            var newDice = Instantiate(dicePrefab);
            newDice.HideOffMap();
            pool.Add(newDice);
        }
        
        for (var i = 0; i < diceCount; i++)
        {
            var offset = new Vector3(i * spawnSpacing, 0f, 0f);
            pool[i].Roll(transform.position + offset);
            activeDice.Add(pool[i]);
        }

        OnRoll?.Invoke();
    }
}