using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private int score;
    private int gameTime;
    private int currentStage = 0; // 현재 스테이지
    private Coroutine gameTimerCoroutine;
    private int comboCount = 0; // 콤보 카운트

    #region 점수와 시간
    // 점수 
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UIManager.Instance.UpdateScoreText(score);
        }
    }

    // 시간
    public int GameTime
    {
        get { return gameTime; }
        set
        {
            gameTime = Mathf.Max(0, value); // 시간은 0 이하로 내려가지 않도록 설정
            UIManager.Instance.UpdateTimerText(gameTime);
            if (gameTime <= 0)
            {
                OnTimeOut();
            }
        }
    }
    #endregion

    private void Start()
    {
        UIManager.Instance.startButton.onClick.AddListener(OnStartButtonClicked); // 버튼 클릭 시 게임 시작
        UIManager.Instance.replayButton.onClick.AddListener(RePlay); // 리플레이 버튼 클릭 시 게임 재시작
        UIManager.Instance.backButton.onClick.AddListener(OnBackButtonClicked); // 백 버튼 클릭 시 처음 화면
        UIManager.Instance.nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked); // 다음 스테이지 버튼 클릭 시 다음 스테이지 시작
        UIManager.Instance.homeButton.onClick.AddListener(OnHomeButtonClicked); // 홈 버튼 클릭 시 처음 화면
    }

    // 타이머
    private IEnumerator UpdateGameTime()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(1f);
            GameTime -= 1;
        }
    }

    #region start & end & replay
    private void GameStart()
    {
        // 게임 시작

        Score = 0;
        gameTime = 30; // 초기 시간 설정
        comboCount = 0;

        UIManager.Instance.ShowGameUI();
        SoundManager.Instance.PlayGameStartSound();
        currentStage = 1;
        StartStage(currentStage);

        // 타이머 시작
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }
        gameTimerCoroutine = StartCoroutine(UpdateGameTime());
    }

    private void RePlay()
    {
        Debug.Log("RePlay");
        Score = 0;
        ResetCombo();
        GameStart();
    }
    #endregion

    #region Btn & UI
    private void OnStartButtonClicked()
    {
        GameStart();
    }

    private void OnBackButtonClicked()
    {
        StageManager.Instance.ClearCards();
        UIManager.Instance.OnBackButtonClicked();
    }

    private void OnHomeButtonClicked()
    {
        StageManager.Instance.ClearCards();
        UIManager.Instance.OnHomeButtonClicked();
    }

    private void OnNextLevelButtonClicked()
    {
        UIManager.Instance.OnNextLevelButtonClicked();
        StartNextStage();
    }

    private void OnTimeOut()
    {
        StageManager.Instance.ClearCards();
        UIManager.Instance.OnTimeOut();
    }
    #endregion

    #region Stage
    public void StartNextStage()
    {
        UIManager.Instance.HideStageCompletePanel();


        currentStage++;
        if (currentStage <= 3) // 스테이지 3까지 진행
        {
            StartStage(currentStage);

            Score = 0;
            ResetCombo();
        }
        else
        {
            UIManager.Instance.HideStageCompletePanel();
            UIManager.Instance.nextLevelButton.gameObject.SetActive(false);
        }
    }

    private void StartStage(int stage)
    {
        switch (stage)
        {
            case 1:
                gameTime = 30;
                StageManager.Instance.StartStage(2, 2);
                UIManager.Instance.UpdateStageText(currentStage, "2x2");
                break;
            case 2:
                gameTime = 40;
                StageManager.Instance.StartStage(2, 4);
                UIManager.Instance.UpdateStageText(currentStage, "2x4");
                break;
            case 3:
                gameTime = 50;
                StageManager.Instance.StartStage(3, 6);
                UIManager.Instance.UpdateStageText(currentStage, "3x6");
                break;
        }

        // 타이머 재설정
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }
        gameTimerCoroutine = StartCoroutine(UpdateGameTime());
    }

    public void OnStageComplete()
    {
        bool isLastStage = (currentStage == 3);
        SoundManager.Instance.PlayStageClearSound();
        UIManager.Instance.ShowStageCompletePanel(score, isLastStage);
    }

    #endregion

    #region Score
    public void AddScore(int points)
    {
        Score += points;
    }

    public void AddCombo()
    {
        comboCount++;
        if (comboCount == 2)
        {
            AddScore(5);
        }
        else if (comboCount >= 3)
        {
            AddScore(10);
        }
    }

    public void ResetCombo()
    {
        comboCount = 0;
    }

    public void DeductScore(int points)
    {
        if (score > 0)
        {
            Score -= points;
        }
    }
    #endregion


}
