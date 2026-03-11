using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardMatcher
{
    void OnCardFlipped(Card card);
    bool IsMatching(Card first, Card second);
}

public class SimpleCardMatcher : MonoBehaviour, ICardMatcher
{
    public void OnCardFlipped(Card card)
    {
        throw new System.NotImplementedException();
    }

    public bool IsMatching(Card first, Card second)
    {
        if (first == null || second == null) return false;
        return first.PairId == second.PairId;
    }
}
