using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int PairId { get; private set; }
    [SerializeField] private Image frontImage;

    public void Setup(int pairId, Sprite sprite)
    {
        PairId = pairId;
        frontImage.sprite = sprite;
    }
}