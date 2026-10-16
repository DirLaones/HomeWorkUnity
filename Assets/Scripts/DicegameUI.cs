using TMPro;
using UnityEngine;

public class DiceGameUI : MonoBehaviour
{
    [SerializeField] private DiceSpawner spawner;
    [SerializeField] private CheckFace checkFace;
    
    [SerializeField] private TextMeshProUGUI targetScoreText;

    [SerializeField] private TextMeshProUGUI currentScoreText;
    
    [SerializeField] private TextMeshProUGUI resultText;

    private int targetScore;

    private void Start()
    {
        GenerateNewTarget();
        UpdateUI(0, false);
    }

    private void OnEnable()
    {
        spawner.OnRoll += HandleRollStarted;
    
        checkFace.OnScoreCounted += HandleScoreCalculated;
    }

    private void OnDisable()
    {
        spawner.OnRoll -= HandleRollStarted;
    
        checkFace.OnScoreCounted -= HandleScoreCalculated;
    }

    private void HandleRollStarted()
    {
        GenerateNewTarget();
        currentScoreText.text = "Выпало: ...";

        resultText.text = "Ожидание остановки...";
    }

    private void HandleScoreCalculated(int totalScore)
    {
        UpdateUI(totalScore, true);
    }

    private void GenerateNewTarget()
    {
        int count = spawner != null ? spawner.DiceCount : 1;
        targetScore = Random.Range(0, count * 5 + 1);

        if (targetScoreText != null)
            targetScoreText.text = $"Нужно для победы: {targetScore}";
    }

    private void UpdateUI(int playerScore, bool isFinished)
    {
        targetScoreText.text = $"Нужно для победы: {targetScore}";
        
        currentScoreText.text = isFinished ? $"Выпало: {playerScore}" : "Выпало: 0";


        if (!isFinished)
        {
            resultText.text = "Нажмите Space для броска";
            return;
        }

        if (playerScore > targetScore)
        {
            resultText.text = "Результат: Победа!";
        }
        else if (playerScore == targetScore)
        {
            resultText.text = "Результат: Ничья!";
        }
        else
        {
            resultText.text = "Результат: Поражение!";
        }
        
    }
}