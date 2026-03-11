using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private GameObject frontSide;
    [SerializeField] private GameObject backSide;
    [SerializeField] private RectTransform visualRoot;

    [SerializeField] private float flipDuration = 0.8f;
    
    
    [SerializeField] private float flipBackDuration = 0.70f;
    [SerializeField] private float flipFrontDuration = 0.45f;


    private Button button;
    private bool isFlipped = false;
    private bool isAnimating = false;

    private IGameState gameState;
    private IAudioService audioService;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void Start()
    {
        gameState = GameManager.Instance.GameTurnStateService;
        audioService = GameManager.Instance.AudioService;

        frontSide.SetActive(true);
        backSide.SetActive(false);
        visualRoot.localRotation = Quaternion.identity;

        StartCoroutine(AutoFlipToBack());
    }

    private IEnumerator AutoFlipToBack()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FlipToBack());
    }

    public void OnClicked()
    {
        var card = GetComponent<Card>();
        if (card == null) return;

        if (!isFlipped || !gameState.CanFlipMore) return;

        audioService.PlayAudio(AudioType.ButtonClick);
        StartCoroutine(FlipToFront());
        gameState.AddFlippedCard(card);
    }

    public IEnumerator FlipToBack()
    {
        isAnimating = true;
        isFlipped = true;

        Quaternion startRot = visualRoot.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 180, 0);

        float t = 0f;
        while (t < flipBackDuration)
        {
            t += Time.deltaTime;
            float lerp = t / flipBackDuration;

            visualRoot.localRotation = Quaternion.Slerp(startRot, endRot, lerp);

            if (lerp >= 0.5f && frontSide.activeSelf)
            {
                frontSide.SetActive(false);
                backSide.SetActive(true);
            }

            yield return null;
        }

        visualRoot.localRotation = endRot;
        isAnimating = false;
    }


    public IEnumerator FlipToFront()
    {
        isAnimating = true;
        isFlipped = false;

        Quaternion startRot = visualRoot.localRotation;
        Quaternion endRot = Quaternion.identity;

        float t = 0f;
        while (t < flipFrontDuration)
        {
            t += Time.deltaTime;
            float lerp = t / flipFrontDuration;

            visualRoot.localRotation = Quaternion.Slerp(startRot, endRot, lerp);

            if (lerp >= 0.5f && backSide.activeSelf)
            {
                backSide.SetActive(false);
                frontSide.SetActive(true);
            }

            yield return null;
        }

        visualRoot.localRotation = endRot;
        isAnimating = false;
    }

}
