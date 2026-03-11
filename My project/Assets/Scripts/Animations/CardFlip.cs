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

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCardClicked);
    }

    private void Start()
    {
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

    public void OnCardClicked()
    {
        Debug.Log("clicked back image");
        if (isAnimating || !isFlipped) return;
        StartCoroutine(FlipToFront());
    }

    private IEnumerator FlipToBack()
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
    }

    private IEnumerator FlipToFront()
    {
        isAnimating = true;
        isFlipped = false;

        Quaternion startRot = visualRoot.localRotation;
        Quaternion endRot = Quaternion.identity;

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flipDuration;
            visualRoot.localRotation = Quaternion.Slerp(startRot, endRot, t);

            if (t >= 0.5f && backSide.activeSelf)
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
