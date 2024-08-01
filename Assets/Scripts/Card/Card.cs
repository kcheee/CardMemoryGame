using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public enum Suit { Spade, Diamond, Heart, Club }
    public enum Rank { A = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K }

    [SerializeField] private Suit cardSuit;
    [SerializeField] private Rank cardRank;

    private bool isFlipped = false;

    public void Flip()
    {
        isFlipped = !isFlipped;
        
        if (isFlipped)
        {
            SoundManager.Instance.PlayCardFlipSound();
            transform.DORotate(new Vector3(0, 0, 0), 0.3f); // 카드 회전
        }
        else
        {
            transform.DORotate(new Vector3(0,0,180), 0.3f); // 원래 상태로 회전
        }
    }

    public Suit GetSuit() => cardSuit;
    public Rank GetRank() => cardRank;


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("d");
        if (!isFlipped)
        {
            Flip();
            GameController.Instance.OnCardFlipped(this);
        }
    }

}
