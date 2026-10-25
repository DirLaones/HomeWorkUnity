using System;
using UnityEngine;

[Serializable]
public class DiceGameModel
{
    public int DiceCount { get; set; }
    public int WinScore { get; set; }
    public int DrawScore { get; set; }
    public int LoseScore { get; set; }
    public int RolledScore { get; set; }

    public DiceGameModel(int defaultDiceCount = 3)
    {
        DiceCount = Mathf.Max(1, defaultDiceCount);
        GenerateRandomThresholds();
    }

    public void GenerateRandomThresholds()
    {
        var minPossible = DiceCount;
        var maxPossible = DiceCount * 6;
        var range = maxPossible - minPossible;

        if (range >= 3)
        {
            var step = range / 3;
            LoseScore = minPossible;
            DrawScore = UnityEngine.Random.Range(minPossible + 1, minPossible + step + 1);
            WinScore = UnityEngine.Random.Range(DrawScore + 1, maxPossible);
        }
        else
        {
            LoseScore = minPossible;
            DrawScore = minPossible + 1;
            WinScore = maxPossible;
        }
    }

    public string EvaluateResult(int score)
    {
        RolledScore = score;
        if (score >= WinScore)
            return "Победа!";
        if (score >= DrawScore)
            return "Ничья!";
        return "Поражение!";
    }
}