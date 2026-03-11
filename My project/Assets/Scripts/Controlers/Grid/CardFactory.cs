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
        if (pairsNeeded > uniqueImages.Count) pairsNeeded = uniqueImages.Count;

        // Build pairs WITH correct pairId
        var pairs = new List<(Sprite sprite, int pairId)>();

        for (int i = 0; i < pairsNeeded; i++)
        {
            pairs.Add((uniqueImages[i], i));
            pairs.Add((uniqueImages[i], i));
        }

        // Shuffle
        for (int i = pairs.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (pairs[i], pairs[rnd]) = (pairs[rnd], pairs[i]);
        }

        // Spawn cards
        for (int i = 0; i < pairs.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, parent);

            var card = cardObj.GetComponent<Card>();
            if (card != null)
            {
                card.Setup(pairs[i].pairId, pairs[i].sprite);
            }

            onCardCreated?.Invoke(cardObj, pairs[i].sprite);
        }
    }

}
