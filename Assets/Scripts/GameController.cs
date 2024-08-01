using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region �̱���
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private Coroutine cardMatchCoroutine;

    // ī�尡 Ŭ���Ǿ��� �� ȣ��
    public void OnCardFlipped(Card card)
    {
        // �̹� �� ���� ī�尡 ���õ� ���¶�� ���� �ڷ�ƾ�� �����ϰ� ��� ó��
        if (cardMatchCoroutine != null)
        {
            StopCoroutine(cardMatchCoroutine);
            ResetFlippedCardsImmediately();
        }

        // ù ��° ī�� ����
        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        // �� ��° ī�� ����
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = card;
            CheckForMatch();
        }
    }

    // �� ���� ī�带 Ȯ��
    private void CheckForMatch()
    {
        if (firstFlippedCard.GetSuit() == secondFlippedCard.GetSuit() &&
            firstFlippedCard.GetRank() == secondFlippedCard.GetRank())
        {
            cardMatchCoroutine = StartCoroutine(SuccessFlippedCards());
        }
        else
        {
            cardMatchCoroutine = StartCoroutine(ResetFlippedCards());
        }
    }

    // �� ���� ī�尡 �´ٸ�
    IEnumerator SuccessFlippedCards()
    {
        yield return new WaitForSeconds(0.5f);

        // ī�尡 ���ٸ� �������
        firstFlippedCard.gameObject.SetActive(false);
        secondFlippedCard.gameObject.SetActive(false);

        // ���� �߰�
        GameManager.Instance.AddScore(10);
        GameManager.Instance.AddCombo();

        firstFlippedCard = null;
        secondFlippedCard = null;
        cardMatchCoroutine = null;

        SoundManager.Instance.PlayCardMatchSound();
        StageManager.Instance.CheckAllCardsMatched();
    }

    // �� ���� ī�尡 �ٸ��ٸ�
    IEnumerator ResetFlippedCards()
    {
        yield return new WaitForSeconds(0.5f);

        firstFlippedCard.Flip();
        secondFlippedCard.Flip();

        GameManager.Instance.DeductScore(5); // �г�Ƽ ����
        GameManager.Instance.ResetCombo();

        firstFlippedCard = null;
        secondFlippedCard = null;

        cardMatchCoroutine = null;
    }


    /// <summary>
    /// ���� ������ �� ���� ī�带 ��� ���� ���·� �ǵ����ϴ�.
    /// �� ī�尡 ��ġ�ϸ� ������ �߰��ϰ� �޺��� ������Ű��, 
    /// ��ġ���� ������ ������ �����ϰ� �޺��� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void ResetFlippedCardsImmediately()
    {
        if (firstFlippedCard != null && secondFlippedCard != null)
        {
            firstFlippedCard.Flip();
            secondFlippedCard.Flip();

            if (firstFlippedCard.GetSuit() == secondFlippedCard.GetSuit() &&
                firstFlippedCard.GetRank() == secondFlippedCard.GetRank())
            {
                firstFlippedCard.gameObject.SetActive(false);
                secondFlippedCard.gameObject.SetActive(false);

                GameManager.Instance.AddScore(10);
                GameManager.Instance.AddCombo();
            }
            else
            {
                GameManager.Instance.DeductScore(5);
                GameManager.Instance.ResetCombo();
            }
        }

        firstFlippedCard = null;
        secondFlippedCard = null;

        cardMatchCoroutine = null;
    }
}
