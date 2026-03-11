using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;
    [SerializeField] private RectTransform visualRoot; // assign VisualRoot here

    [SerializeField] private float flipDuration = 0.4f;
    [SerializeField] private float delayBeforeFlip = 1.0f;

    private Button button;
    private bool isFlipped = false;
    private bool isAnimating = false;

// NEW: dependency
    private IGameState gameState;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("No Button on " + gameObject.name, gameObject);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void Start()
    {
        gameState = GameManager.Instance.GameTurnStateService;
        frontSide.SetActive(true);
        backSide.SetActive(false);
        visualRoot.localRotation = Quaternion.identity;

        StartCoroutine(AutoFlipToBackAfterDelay());
    }

    private IEnumerator AutoFlipToBackAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFlip);
        yield return StartCoroutine(FlipToBack());
    }

    public void OnClicked()
    {
        var card = GetComponent<Card>();
        int id = card != null ? card.PairId : -1;

        Debug.Log($"[Click] Card clicked! PairId = {id} | Name = {gameObject.name}");

        if (isAnimating || !isFlipped || !gameState.CanFlipMore) return;

        StartCoroutine(FlipToFront());
        gameState.AddFlippedCard(card);
    }

    public IEnumerator FlipToBack()
    {
        isAnimating = true;
        isFlipped = true;

        Quaternion startRot = visualRoot.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 180, 0);

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flipDuration;
            visualRoot.localRotation = Quaternion.Slerp(startRot, endRot, t);

            if (t >= 0.5f && frontSide.activeSelf)
            {
                frontSide.SetActive(false);
                backSide.SetActive(true);
            }

            yield return null;
        }

        visualRoot.localRotation = endRot;
        isAnimating = false;
        Debug.Log(gameObject.name + " finished flip to " + (isFlipped ? "BACK" : "FRONT"));
    }

    public IEnumerator FlipToFront()
    {
        isAnimating = true;
        isFlipped = false;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.identity;

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flipDuration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);

            if (t >= 0.5f && backSide.activeSelf)
            {
                backSide.SetActive(false);
                frontSide.SetActive(true);
            }

            yield return null;
        }

        transform.localRotation = endRot;
        isAnimating = false;
        Debug.Log(gameObject.name + " finished flip to " + (isFlipped ? "BACK" : "FRONT"));
    }
}