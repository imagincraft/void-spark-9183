using System.Collections;
using UnityEngine;
using TMPro;

public interface IUiManagerService
{
    void SetScore(int score);
    void SetTurns(int turns);
    void OpenPanel(GameObject panel);
    void ClosePanel(GameObject panel);
}

public class UiManager : MonoBehaviour, IUiManagerService
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    public void SetScore(int score)
    {
        scoreText.text = "" + score;
    }

    public void SetTurns(int turns)
    {
        turnText.text = "" + turns;
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;

        StartCoroutine(ScaleOverTime(panel.transform, Vector3.one, 0.25f));

    }

    public void ClosePanel(GameObject panel)
    {
        StartCoroutine(CloseRoutine(panel));
    }

    private IEnumerator CloseRoutine(GameObject panel)
    {
        yield return ScaleOverTime(panel.transform, Vector3.zero, 0.25f);
        panel.SetActive(false);
    }
    
    private IEnumerator ScaleOverTime(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Smooth easing
            t = Mathf.SmoothStep(0, 1, t);

            target.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }
}