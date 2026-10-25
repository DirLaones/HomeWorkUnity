using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceGameUI : MonoBehaviour
{
    [Header("Ссылки на логику костей")]
    [SerializeField] private DiceSpawner spawner;
    [SerializeField] private CheckFace checkFace;

    [Header("Отображение значений (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI diceCountText;
    [SerializeField] private TextMeshProUGUI winScoreText;
    [SerializeField] private TextMeshProUGUI drawScoreText;
    [SerializeField] private TextMeshProUGUI loseScoreText;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Поля ввода (TMP_InputField)")]
    [SerializeField] private TMP_InputField diceCountInput;
    [SerializeField] private TMP_InputField winScoreInput;
    [SerializeField] private TMP_InputField drawScoreInput;
    [SerializeField] private TMP_InputField loseScoreInput;

    [Header("Кнопки")]
    [SerializeField] private Button rerollButton;

    private DiceGameModel model;

    public DiceGameModel Model => model;

    private void Awake()
    {
        var initialDiceCount = spawner != null ? spawner.DiceCount : 3;
        Init(new DiceGameModel(initialDiceCount));
        
        if (rerollButton != null)
            rerollButton.onClick.AddListener(OnRerollButtonClicked);

        if (diceCountInput != null)
            diceCountInput.onEndEdit.AddListener(OnDiceCountInputChanged);

        if (winScoreInput != null)
            winScoreInput.onEndEdit.AddListener(OnWinScoreInputChanged);

        if (drawScoreInput != null)
            drawScoreInput.onEndEdit.AddListener(OnDrawScoreInputChanged);

        if (loseScoreInput != null)
            loseScoreInput.onEndEdit.AddListener(OnLoseScoreInputChanged);
    }

    public void Init(DiceGameModel gameModel)
    {
        model = gameModel;
        UpdateUI(isRolling: false);
    }

    private void OnEnable()
    {
        if (spawner != null)
            spawner.OnRoll += HandleRollStarted;

        if (checkFace != null)
            checkFace.OnScoreCounted += HandleScoreCalculated;
    }

    private void OnDisable()
    {
        if (spawner != null)
            spawner.OnRoll -= HandleRollStarted;

        if (checkFace != null)
            checkFace.OnScoreCounted -= HandleScoreCalculated;
    }
    public void OnDiceCountInputChanged(string text)
    {
        if (int.TryParse(text, out var count) && count > 0)
        {
            model.DiceCount = count;
            if (spawner != null)
                spawner.DiceCount = count;

            model.GenerateRandomThresholds();
            UpdateUI(isRolling: false);
        }
    }

    public void OnWinScoreInputChanged(string text)
    {
        if (int.TryParse(text, out var score))
        {
            model.WinScore = score;
            UpdateThresholdTexts();
        }
    }

    public void OnDrawScoreInputChanged(string text)
    {
        if (int.TryParse(text, out var score))
        {
            model.DrawScore = score;
            UpdateThresholdTexts();
        }
    }

    public void OnLoseScoreInputChanged(string text)
    {
        if (int.TryParse(text, out var score))
        {
            model.LoseScore = score;
            UpdateThresholdTexts();
        }
    }

    public void OnRerollButtonClicked()
    {
        Reroll();
    }

    public void Reroll()
    {
        model.GenerateRandomThresholds();
        UpdateUI(isRolling: true);

        if (spawner != null)
            spawner.RollDice(model.DiceCount);
    }

    private void HandleRollStarted()
    {
        UpdateUI(isRolling: true);
    }

    private void HandleScoreCalculated(int totalScore)
    {
        var outcome = model.EvaluateResult(totalScore);

        if (currentScoreText != null)
            currentScoreText.text = $"Выпало: {totalScore}";

        if (resultText != null)
            resultText.text = $"Результат: {outcome}";
    }

    private void UpdateUI(bool isRolling)
    {
        if (diceCountText != null)
            diceCountText.text = $"Число кубиков: {model.DiceCount}";

        if (diceCountInput != null && diceCountInput.text != model.DiceCount.ToString())
            diceCountInput.text = model.DiceCount.ToString();

        UpdateThresholdTexts();

        if (currentScoreText != null)
            currentScoreText.text = isRolling ? "Выпало: ..." : "Выпало: 0";

        if (resultText != null)
            resultText.text = isRolling ? "Ожидание остановки..." : "Нажмите бросок";
    }

    private void UpdateThresholdTexts()
    {
        if (winScoreText != null)
            winScoreText.text = $"Победа: >= {model.WinScore}";

        if (drawScoreText != null)
            drawScoreText.text = $"Ничья: >= {model.DrawScore}";

        if (loseScoreText != null)
            loseScoreText.text = $"Поражение: < {model.DrawScore}";

        if (winScoreInput != null && winScoreInput.text != model.WinScore.ToString())
            winScoreInput.text = model.WinScore.ToString();

        if (drawScoreInput != null && drawScoreInput.text != model.DrawScore.ToString())
            drawScoreInput.text = model.DrawScore.ToString();

        if (loseScoreInput != null && loseScoreInput.text != model.LoseScore.ToString())
            loseScoreInput.text = model.LoseScore.ToString();
    }
}