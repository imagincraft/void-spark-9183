using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MatchingHandler : MonoBehaviour
{
    //Services
    private ICardMatcher matcher;
    private IGameState gameState;
    private IScoreService scoreService;
    private IAudioService audioService;
    private IUiManagerService uiManager;

    [SerializeField] private float revealDelay = 0.2f;
    [SerializeField] private float flipBackDelay = 0.1f;
    private int matchedPairs = 0;
    private int totalPairs = 0;
    private int comboCount = 0;
    private float comboMultiplier = 1f;


    [SerializeField] public GameObject wonPanel;

    private Queue<(Card, Card)> matchQueue = new Queue<(Card, Card)>();
    private bool isProcessingQueue = false;

    private void Start()
    {
        matcher = GameManager.Instance.CardMatcherService;
        gameState = GameManager.Instance.GameTurnStateService;
        scoreService = GameManager.Instance.ScoreService;
        audioService = GameManager.Instance.AudioService;
        uiManager = GameManager.Instance.UiManagerService;

        if (matcher == null || gameState == null)
        {
            Debug.LogError("Matcher or GameState missing!", this);
        }
    }


    private void OnEnable()
    {
        LevelEvents.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        LevelEvents.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelData level)
    {
        matchQueue.Clear();
        isProcessingQueue = false;

        matchedPairs = 0;
        totalPairs = (level.rows * level.columns) / 2;
        
        comboCount = 0;
        comboMultiplier = 1f;
    }


    private void Update()
    {
        var flipped = gameState.GetFlippedCards();

        if (flipped.Count >= 2)
        {
            var first = flipped[0];
            var second = flipped[1];

            matchQueue.Enqueue((first, second));

            // Remove them from the flipped list
            (gameState as GameTurnState)?.RemoveFirstTwo();

            if (!isProcessingQueue)
                StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessingQueue = true;

        while (matchQueue.Count > 0)
        {
            var pair = matchQueue.Dequeue();
            yield return StartCoroutine(CheckMatchRoutine(pair.Item1, pair.Item2));
        }

        isProcessingQueue = false;
    }


    // private bool isCheckingMatch = false;

    private IEnumerator CheckMatchRoutine(Card first, Card second)
    {
        Debug.Log("[Matching] Starting check routine");

        yield return new WaitForSeconds(revealDelay);

        bool match = matcher.IsMatching(first, second);
        scoreService.AddTurn();

        if (match)
        {
            audioService.PlayAudio(AudioType.ImageClicking);

            comboCount++;
            comboMultiplier = 1f + (comboCount - 1) * 0.5f; // each combo adds +0.5

            int gainedScore = Mathf.RoundToInt(1 * comboMultiplier);
            scoreService.AddScore(gainedScore);

            matchedPairs++;

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("ALL MATCHES FOUND!");
                yield return new WaitForSeconds(1.5f);
                audioService.PlayAudio(AudioType.PanelOpen);
                uiManager.OpenPanel(wonPanel);
            }

            Debug.Log("[Matching] MATCH! Keeping face up");
            first.GetComponent<Button>().interactable = false;
            second.GetComponent<Button>().interactable = false;
        }
        else
        {
            audioService.PlayAudio(AudioType.Error);

            comboCount = 0;
            comboMultiplier = 1f;

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

        /*
        gameState.ClearFlippedCards();
        isCheckingMatch = false;*/
    }
}