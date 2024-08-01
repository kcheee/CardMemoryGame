using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region 싱글톤
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private Coroutine cardMatchCoroutine;

    // 카드가 클릭되었을 때 호출
    public void OnCardFlipped(Card card)
    {
        // 이미 두 개의 카드가 선택된 상태라면 이전 코루틴을 종료하고 즉시 처리
        if (cardMatchCoroutine != null)
        {
            StopCoroutine(cardMatchCoroutine);
            ResetFlippedCardsImmediately();
        }

        // 첫 번째 카드 선택
        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        // 두 번째 카드 선택
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = card;
            CheckForMatch();
        }
    }

    // 두 개의 카드를 확인
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

    // 두 개의 카드가 맞다면
    IEnumerator SuccessFlippedCards()
    {
        yield return new WaitForSeconds(0.5f);

        // 카드가 같다면 사라지게
        firstFlippedCard.gameObject.SetActive(false);
        secondFlippedCard.gameObject.SetActive(false);

        // 점수 추가
        GameManager.Instance.AddScore(10);
        GameManager.Instance.AddCombo();

        firstFlippedCard = null;
        secondFlippedCard = null;
        cardMatchCoroutine = null;

        SoundManager.Instance.PlayCardMatchSound();
        StageManager.Instance.CheckAllCardsMatched();
    }

    // 두 개의 카드가 다르다면
    IEnumerator ResetFlippedCards()
    {
        yield return new WaitForSeconds(0.5f);

        firstFlippedCard.Flip();
        secondFlippedCard.Flip();

        GameManager.Instance.DeductScore(5); // 패널티 점수
        GameManager.Instance.ResetCombo();

        firstFlippedCard = null;
        secondFlippedCard = null;

        cardMatchCoroutine = null;
    }


    /// <summary>
    /// 현재 뒤집힌 두 개의 카드를 즉시 원래 상태로 되돌립니다.
    /// 두 카드가 일치하면 점수를 추가하고 콤보를 증가시키며, 
    /// 일치하지 않으면 점수를 차감하고 콤보를 초기화합니다.
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
