using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridParent;           // Drag CardGrid here
    [SerializeField] private GameObject cardPrefab;          // Drag Card prefab here

    [Header("Test Layouts")]
    public Vector2Int gridSize = new Vector2Int(4, 4);       // Change this in Inspector to test

    private List<GameObject> spawnedCards = new List<GameObject>();

    private void Start()
    {
        GenerateGrid(gridSize.x, gridSize.y);
    }

    // Call this anytime to change layout (2x2, 3x3, 5x6, etc.)
    public void GenerateGrid(int rows, int columns)
    {
        // Clear old cards
        foreach (GameObject card in spawnedCards)
            Destroy(card);
        spawnedCards.Clear();

        int totalCards = rows * columns;
        if (totalCards % 2 == 1) columns--; // keep even number of cards

        // Create simple placeholder cards
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject card = Instantiate(cardPrefab, gridParent);
            spawnedCards.Add(card);
        }

        // AUTO-SCALE to perfectly fit the CardGrid container
        AutoScaleToFit(rows, columns);
    }

    private void AutoScaleToFit(int rows, int columns)
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform gridRect = gridParent.GetComponent<RectTransform>();

        if (grid == null || gridRect == null) return;

        float availableWidth  = gridRect.rect.width  - (grid.spacing.x * (columns - 1));
        float availableHeight = gridRect.rect.height - (grid.spacing.y * (rows - 1));

        float cellWidth  = availableWidth  / columns;
        float cellHeight = availableHeight / rows;

        // Keep cards perfectly square (best look)
        float finalSize = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(finalSize, finalSize);
    }

    // Optional: public method so you can call from buttons later
    public void ChangeLayout(int rows, int columns)
    {
        gridSize = new Vector2Int(rows, columns);
        GenerateGrid(rows, columns);
    }
}