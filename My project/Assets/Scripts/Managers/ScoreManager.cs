using System;
using UnityEngine;

public interface IScoreService
{
    int Score { get; }
    int Turns { get; }

    void AddScore(int amount);
    void AddTurn();
    void Reset();
}

public class ScoreManager : MonoBehaviour, IScoreService
{
    public int Score { get; private set; }
    public int Turns { get; private set; }

    private IUiManagerService uiService ;

    private void Start()
    {
        uiService = GameManager.Instance.UiManagerService;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        uiService.SetScore(Score);
    }

    public void AddTurn()
    {
        Turns++;
        uiService.SetTurns(Turns);
    }

    public void Reset()
    {
        Score = 0;
        Turns = 0;
        uiService.SetScore(0);
        uiService.SetTurns(0);
    }
}
