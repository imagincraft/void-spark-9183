using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Transform gridContainer;

    [SerializeField] private Card cardComponentPrefab; // reference to Card script on prefab (for GetComponent)

    // [Header("Dependencies (assign concrete implementations here)")]
    // [SerializeField] private CardFactory cardFactoryComponent;          // ← concrete class
    // [SerializeField] private GridLayoutConfigurator layoutConfiguratorComponent;  // ← concrete class

    private ICardFactory cardFactory; // runtime interface reference
    private IGridLayoutConfigurator layoutConfigurator;
    private ILevelManagerService levelManagerService;

    [Header("Images")] [SerializeField] private List<Sprite> uniqueCardImages;

    [Header("Grid Size")] [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;

    private readonly List<GameObject> spawnedCards = new List<GameObject>();

    private void Awake()
    {
        // Get the interfaces from the concrete components
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
        GenerateGrid(level.rows, level.columns);
    }

    private void Start()
    {
        cardFactory = GameManager.Instance.CardFactoryService;
        layoutConfigurator = GameManager.Instance.GridLayoutConfiguratorService;
        levelManagerService = GameManager.Instance.LevelManagerService;

        if (cardFactory == null) Debug.LogError("CardFactory missing or not implementing ICardFactory!", this);
        if (layoutConfigurator == null) Debug.LogError("LayoutConfigurator missing!", this);
        levelManagerService.LoadLevel(1);
        // GenerateGrid(rows, columns);
    }

    public void GenerateGrid(int rowCount, int colCount)
    {
        ClearPreviousCards();

        int totalCards = rowCount * colCount;

        // Spawn cards via factory
        /*cardFactory.CreateCards(
            gridContainer,
            totalCards,
            uniqueCardImages,
            (cardObj, sprite) =>
            {
                spawnedCards.Add(cardObj);
                var card = cardObj.GetComponent<Card>();
                if (card != null) card.SetImage(sprite);
                else Debug.LogWarning("Card component missing on spawned object!", cardObj);
            });*/
        
        
        cardFactory.CreateCards(
            gridContainer,
            totalCards,
            uniqueCardImages,
            (cardObj, sprite) =>
            {
                spawnedCards.Add(cardObj);

                var card = cardObj.GetComponent<Card>();
                if (card == null)
                {
                    Debug.LogWarning("Card component missing on spawned object!", cardObj);
                }
            });

        // Apply layout
        var gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        var rect = gridContainer.GetComponent<RectTransform>();

        if (gridLayout != null && rect != null)
        {
            layoutConfigurator.ConfigureCellSize(gridLayout, rect, rowCount, colCount);
        }
    }

    private void ClearPreviousCards()
    {
        foreach (var card in spawnedCards)
            if (card != null)
                Destroy(card);

        spawnedCards.Clear();
    }

    // Optional: public method for changing layout at runtime
    public void ChangeLayout(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;
        GenerateGrid(newRows, newColumns);
    }
}