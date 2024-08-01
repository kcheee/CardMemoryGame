using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Text Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI stageScoreText; // 스테이지 완료 텍스트

    [Header("Buttons")]
    public Button startButton;
    public Button replayButton;
    public Button backButton;
    public Button nextLevelButton;
    public Button homeButton;

    [Header("Panels")]
    public GameObject stageCompletePanel;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        HideAllUI();
        startButton.gameObject.SetActive(true);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Point\n" + score;
    }

    public void UpdateTimerText(int time)
    {
        timerText.text = "Time: " + time.ToString() + " sec";
    }

    public void UpdateStageText(int stage, string stageDimensions)
    {
        stageText.text = "Stage " + stage.ToString() + ": " + stageDimensions;
    }

    public void ShowStageCompletePanel(int score, bool isLastStage)
    {
        stageScoreText.text = "Score: " + score;
        stageCompletePanel.SetActive(true);
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        stageText.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(!isLastStage); // 마지막 스테이지라면 nextLevelButton 비활성화
    }

    public void HideStageCompletePanel()
    {
        stageCompletePanel.SetActive(false);
    }

    public void ShowGameUI()
    {
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        stageText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void ShowEndGameUI()
    {
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void HideGameUI()
    {
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        stageText.gameObject.SetActive(false);
    }

    public void HideAllUI()
    {
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        stageText.gameObject.SetActive(false);
        stageCompletePanel.SetActive(false);
        startButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        ShowGameUI();
    }

    public void OnBackButtonClicked()
    {
        HideAllUI();
        ShowStartButton();
    }

    public void OnHomeButtonClicked()
    {
        HideAllUI();
        ShowStartButton();
    }

    public void OnNextLevelButtonClicked()
    {
        HideStageCompletePanel();
        ShowGameUI();
    }

    public void OnTimeOut()
    {
        HideAllUI();
        replayButton.gameObject.SetActive(true);
    }
}
