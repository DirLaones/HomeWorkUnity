using System.Collections;
using UnityEngine;

public class CheckFace : MonoBehaviour
{
    public event System.Action<int> OnScoreCounted;
    private DiceSpawner spawner;
    [SerializeField] private float startCheckDelay = 0.3f;
    [SerializeField] private float stopThreshold = 0.01f;

    private Coroutine checkCoroutine;


    private void Awake()
    {
        spawner = GetComponent<DiceSpawner>();
    }
    private void OnEnable()
    {
        if (spawner != null)
            spawner.OnRoll += StartChecking;
    }

    private void OnDisable()
    {
        if (spawner != null)
            spawner.OnRoll -= StartChecking;
    }

    private void StartChecking()
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(WaitAndCountRoutine());
    }

    private IEnumerator WaitAndCountRoutine()
    {
        yield return new WaitForSeconds(startCheckDelay);

        while (!AreAllStopped())
        {
            yield return new WaitForFixedUpdate();
        }

        int totalScore = 0;
        for (int i = 0; i < spawner.ActiveDice.Count; i++)
        {
            int faceValue = GetTopFace(spawner.ActiveDice[i].transform);
            totalScore += faceValue;
        }

        OnScoreCounted?.Invoke(totalScore);
    }

    private bool AreAllStopped()
    {
        foreach (Dice dice in spawner.ActiveDice)
        {
            if (!dice.IsStopped(stopThreshold))
                return false;
        }
        return true;
    }

    private int GetTopFace(Transform diceTransform)
    {
        Vector3[] faces =
        {
            diceTransform.forward,   
            diceTransform.up,        
            -diceTransform.right,    
            diceTransform.right,     
            -diceTransform.up,       
            -diceTransform.forward   
        };

        int result = 1;
        float maxDot = -1f;

        for (int i = 0; i < faces.Length; i++)
        {
            float dot = Vector3.Dot(faces[i], Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                result = i + 1;
            }
        }

        return result;
    }
}