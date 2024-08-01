using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region �̱���
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private int score;
    private int gameTime;
    private int currentStage = 0; // ���� ��������
    private Coroutine gameTimerCoroutine;
    private int comboCount = 0; // �޺� ī��Ʈ

    #region ������ �ð�
    // ���� 
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UIManager.Instance.UpdateScoreText(score);
        }
    }

    // �ð�
    public int GameTime
    {
        get { return gameTime; }
        set
        {
            gameTime = Mathf.Max(0, value); // �ð��� 0 ���Ϸ� �������� �ʵ��� ����
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
        UIManager.Instance.startButton.onClick.AddListener(OnStartButtonClicked); // ��ư Ŭ�� �� ���� ����
        UIManager.Instance.replayButton.onClick.AddListener(RePlay); // ���÷��� ��ư Ŭ�� �� ���� �����
        UIManager.Instance.backButton.onClick.AddListener(OnBackButtonClicked); // �� ��ư Ŭ�� �� ó�� ȭ��
        UIManager.Instance.nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked); // ���� �������� ��ư Ŭ�� �� ���� �������� ����
        UIManager.Instance.homeButton.onClick.AddListener(OnHomeButtonClicked); // Ȩ ��ư Ŭ�� �� ó�� ȭ��
    }

    // Ÿ�̸�
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
        // ���� ����

        Score = 0;
        gameTime = 30; // �ʱ� �ð� ����
        comboCount = 0;

        UIManager.Instance.ShowGameUI();
        SoundManager.Instance.PlayGameStartSound();
        currentStage = 1;
        StartStage(currentStage);

        // Ÿ�̸� ����
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
        if (currentStage <= 3) // �������� 3���� ����
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

        // Ÿ�̸� �缳��
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
