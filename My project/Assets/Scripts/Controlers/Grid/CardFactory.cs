using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICardFactory
{
    void CreateCards(
        Transform parent,
        int totalCards,
        IReadOnlyList<Sprite> uniqueImages,
        System.Action<GameObject, Sprite> onCardCreated);
}
public class CardFactory : MonoBehaviour, ICardFactory
{
    [SerializeField] private GameObject cardPrefab;

    public void CreateCards(
        Transform parent,
        int totalCards,
        IReadOnlyList<Sprite> uniqueImages,
        System.Action<GameObject, Sprite> onCardCreated)
    {
        if (totalCards % 2 == 1) totalCards--;

        int pairsNeeded = totalCards / 2;

        if (pairsNeeded > uniqueImages.Count)
        {
            Debug.LogWarning($"Not enough unique images. Using {uniqueImages.Count} pairs.");
            pairsNeeded = uniqueImages.Count;
        }

        // Build pair list
        var cardImages = new List<Sprite>(totalCards);
        for (int i = 0; i < pairsNeeded; i++)
        {
            cardImages.Add(uniqueImages[i]);
            cardImages.Add(uniqueImages[i]);
        }

        // Shuffle
        for (int i = cardImages.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (cardImages[i], cardImages[rnd]) = (cardImages[rnd], cardImages[i]);
        }

        // Spawn
        for (int i = 0; i < cardImages.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, parent);
            onCardCreated?.Invoke(card, cardImages[i]);
        }
    }
}
