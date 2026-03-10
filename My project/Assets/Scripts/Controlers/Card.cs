using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image frontImage;

    public void SetImage(Sprite sprite)
    {
        if (frontImage != null)
            frontImage.sprite = sprite;
    }
}