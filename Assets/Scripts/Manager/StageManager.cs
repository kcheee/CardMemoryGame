using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region �̱���
    public static StageManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject[] cardPrefabs; // 52���� ī�� �������� ��� �迭
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

    // ī�� ��ġ�� �°� ����
    private void CreateCards(int rows, int cols)
    {
        // �ʿ��� ī�� �� �� ���
        int totalCards = rows * cols;
        if (totalCards > cardPrefabs.Length * 2)
        {
            Debug.LogError("ī�� ����");
            return;
        }

        List<GameObject> selectedCards = new List<GameObject>();

        // ī�� �������� �������� ����
        List<GameObject> shuffledCardPrefabs = new List<GameObject>(cardPrefabs);
        Shuffle(shuffledCardPrefabs);

        // �ʿ��� ī�� �� ����
        for (int i = 0; i < totalCards / 2; i++)
        {
            selectedCards.Add(shuffledCardPrefabs[i]);
            selectedCards.Add(shuffledCardPrefabs[i]);
        }

        // ������ ī�带 �������� ����
        Shuffle(selectedCards);

        // ī�� ��ġ
        float cardSpacingX = 0.75f; // ī�� ���� (X��)
        float cardSpacingZ = 1f; // ī�� ���� (Z��)

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

    // ī�� ����
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
