using System.Collections.Generic;
using UnityEngine;


public interface IGameState
{
    bool CanFlipMore { get; }
    void AddFlippedCard(Card card);
    void ClearFlippedCards();
    IReadOnlyList<Card> GetFlippedCards();
}

public class GameTurnState : MonoBehaviour, IGameState
{
    private readonly List<Card> flippedCards = new List<Card>();

    public bool CanFlipMore => true;

    public void AddFlippedCard(Card card)
    {
        if (card == null || flippedCards.Contains(card)) return;
        flippedCards.Add(card);
    }


    public void ClearFlippedCards()
    {
        flippedCards.Clear();
    }

    public IReadOnlyList<Card> GetFlippedCards()
    {
        return flippedCards.AsReadOnly();
    }
    
    public void RemoveFirstTwo()
    {
        if (flippedCards.Count >= 2)
        {
            flippedCards.RemoveAt(0);
            flippedCards.RemoveAt(0);
        }
    }
}