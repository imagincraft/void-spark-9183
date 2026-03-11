using UnityEngine;
using TMPro;

public interface IUiManagerService
{
    void SetScore(int score);
    void SetTurns(int turns);
}

public class UiManager : MonoBehaviour, IUiManagerService
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void SetTurns(int turns)
    {
        turnText.text = "Turns: " + turns;
    }
}