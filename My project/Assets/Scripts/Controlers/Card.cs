using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int PairId { get; private set; }
    [SerializeField] private Image frontImage;

    public void Setup(int pairId, Sprite sprite)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one; 
        
        PairId = pairId;
        frontImage.sprite = sprite;
    }
}