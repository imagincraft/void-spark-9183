using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchingHandler : MonoBehaviour
{
    private ICardMatcher matcher;
    private IGameState gameState;
    [SerializeField] private float revealDelay = 1f;
    [SerializeField] private float flipBackDelay = 0.6f;

    private void Start()
    {
        matcher = GameManager.Instance.CardMatcherService;
        gameState = GameManager.Instance.GameTurnStateService;
        if (matcher == null || gameState == null)
        {
            Debug.LogError("Matcher or GameState missing!", this);
        }
    }

    private void Update()
    {
        var flipped = gameState.GetFlippedCards();
        if (flipped.Count == 2 && !isCheckingMatch) // prevent multiple coroutines
        {
            isCheckingMatch = true;
            Debug.Log($"[Matching] 2 cards flipped - checking {flipped[0].PairId} vs {flipped[1].PairId}");
            StartCoroutine(CheckMatchRoutine(flipped[0], flipped[1]));
        }
    }

    private bool isCheckingMatch = false;

    private IEnumerator CheckMatchRoutine(Card first, Card second)
    {
        Debug.Log("[Matching] Starting check routine");

        yield return new WaitForSeconds(revealDelay);

        bool match = matcher.IsMatching(first, second);

        if (match)
        {
            Debug.Log("[Matching] MATCH! Keeping face up");
            first.GetComponent<Button>().interactable = false;
            second.GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("[Matching] NO MATCH - flipping back");
            yield return new WaitForSeconds(flipBackDelay);

            var flip1 = first.GetComponent<CardFlip>();
            var flip2 = second.GetComponent<CardFlip>();

            if (flip1 != null)
            {
                yield return flip1.FlipToBack();
            }
            else Debug.LogWarning("No CardFlip on first card");

            if (flip2 != null)
            {
                yield return flip2.FlipToBack();
            }
            else Debug.LogWarning("No CardFlip on second card");
        }

        gameState.ClearFlippedCards();
        isCheckingMatch = false;
    }
}