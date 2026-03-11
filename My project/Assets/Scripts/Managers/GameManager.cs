using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private GridLayoutConfigurator gridLayoutConfigurator;
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private GameTurnState gameTurnState;
    [SerializeField] private SimpleCardMatcher SimpleCardMatcher;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private ScoreManager scoreManager;
    public static GameManager Instance => instance;

    public IDataService DataService;
    public IGridLayoutConfigurator GridLayoutConfiguratorService;
    public ICardFactory CardFactoryService;
    public IGameState GameTurnStateService;
    public ICardMatcher CardMatcherService;
    public ILevelManagerService LevelManagerService;
    public IUiManagerService UiManagerService;
    public IScoreService ScoreService;

    
    public SaveData SaveData{get; private set;}

    void Awake()
    {
        SaveData = SaveSystem.Load();
        // singleton part ...
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ← usually good idea for GameManager

        // ──────────────────────────────────────────────
        // Better null check with names
        if (dataManager == null) Debug.LogError("dataManager is NULL", this);
        if (gridLayoutConfigurator == null) Debug.LogError("gridLayoutConfigurator is NULL", this);
        if (cardFactory == null) Debug.LogError("cardFactory is NULL", this);

        if (dataManager == null || gridLayoutConfigurator == null || cardFactory == null)
        {
            Debug.LogError("GameManager missing required references – stopping.", this);
            // throw new ...   ← comment out for now so you see the logs
            return;
        }

        DataService = dataManager;
        GridLayoutConfiguratorService = gridLayoutConfigurator;
        CardFactoryService = cardFactory;
        GameTurnStateService = gameTurnState;
        CardMatcherService = SimpleCardMatcher;
        LevelManagerService = levelManager;
        UiManagerService = uiManager;
        ScoreService = scoreManager;
    }

    private void Start()
    {
        int savedLevel = SaveData.unlockedLevel;
        LevelManagerService.LoadLevel(savedLevel);
    }
}