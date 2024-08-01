using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region 싱글톤
    public static StageManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject[] cardPrefabs; // 52장의 카드 프리팹을 담는 배열
    private List<GameObject> cards = new List<GameObject>();

    public void StartStage(int rows, int cols)
    {
        ClearCards();
        CreateCards(rows, cols);
    }

    public void ClearCards()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }

    // 카드 위치에 맞게 생성
    private void CreateCards(int rows, int cols)
    {
        // 필요한 카드 쌍 수 계산
        int totalCards = rows * cols;
        if (totalCards > cardPrefabs.Length * 2)
        {
            Debug.LogError("카드 부족");
            return;
        }

        List<GameObject> selectedCards = new List<GameObject>();

        // 카드 프리팹을 무작위로 섞음
        List<GameObject> shuffledCardPrefabs = new List<GameObject>(cardPrefabs);
        Shuffle(shuffledCardPrefabs);

        // 필요한 카드 쌍 선택
        for (int i = 0; i < totalCards / 2; i++)
        {
            selectedCards.Add(shuffledCardPrefabs[i]);
            selectedCards.Add(shuffledCardPrefabs[i]);
        }

        // 선택한 카드를 무작위로 섞음
        Shuffle(selectedCards);

        // 카드 배치
        float cardSpacingX = 0.75f; // 카드 간격 (X축)
        float cardSpacingZ = 1f; // 카드 간격 (Z축)

        float startX = -(cols - 1) / 2f * cardSpacingX;
        float startZ = -(rows - 1) / 2f * cardSpacingZ;

        int cardIndex = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 position = new Vector3(startX + j * cardSpacingX, 0, startZ + i * cardSpacingZ);
                Quaternion rotation = Quaternion.Euler(0, 0, 180);
                GameObject cardObject = Instantiate(selectedCards[cardIndex], position, rotation);

                cards.Add(cardObject);
                cardIndex++;
            }
        }
    }

    // 카드 섞기
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CheckAllCardsMatched()
    {
        if (cards.TrueForAll(card => !card.activeSelf))
        {
            GameManager.Instance.OnStageComplete();
        }
    }
}
