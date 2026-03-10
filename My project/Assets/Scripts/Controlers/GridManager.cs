using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gridContainer;      // ← Drag CardGridContainer here
    [SerializeField] private GameObject cardPrefab;        // ← Drag Card prefab here

    [Header("Images to use (put your unique sprites here)")]
    [SerializeField] private List<Sprite> uniqueCardImages;   // ← Drag your 3+ images here

    [Header("Grid Size")]
    [SerializeField] private int rows ;
    [SerializeField] private int columns ;

    private List<GameObject> spawnedCards = new List<GameObject>();

    void Start()
    {
        CreateGridWithImages(rows, columns);
    }

    public void CreateGridWithImages(int rowCount, int colCount)
    {
        // Clear old cards
        foreach (GameObject card in spawnedCards)
            if (card != null) Destroy(card);
        spawnedCards.Clear();

        // Make sure we have even number of cards
        if ((rowCount * colCount) % 2 == 1) colCount--;

        int totalCards = rowCount * colCount;
        int pairsNeeded = totalCards / 2;

        // Check we have enough unique images
        if (pairsNeeded > uniqueCardImages.Count)
        {
            Debug.LogError("Not enough unique images! Using all available.");
            pairsNeeded = uniqueCardImages.Count;
        }

        // Step 1: Create list with pairs (duplicate each image)
        List<Sprite> cardList = new List<Sprite>();
        for (int i = 0; i < pairsNeeded; i++)
        {
            cardList.Add(uniqueCardImages[i]);
            cardList.Add(uniqueCardImages[i]);   // duplicate = pair
        }

        // Step 2: Shuffle the list
        Shuffle(cardList);

        // Step 3: Spawn cards and assign images
        for (int i = 0; i < cardList.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridContainer);
            Card cardScript = cardObj.GetComponent<Card>();

            cardScript.SetImage(cardList[i]);     // ← This assigns the sprite

            spawnedCards.Add(cardObj);
        }

        // Auto scale (same as before)
        UpdateCellSize(rowCount, colCount);

        Debug.Log($"Spawned {cardList.Count} cards with {pairsNeeded} unique image pairs");
    }

    private void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    private void UpdateCellSize(int rows, int cols)
    {
        var grid = gridContainer.GetComponent<GridLayoutGroup>();
        var rect = gridContainer.GetComponent<RectTransform>();
        if (grid == null || rect == null) return;

        float totalW = rect.rect.width - grid.spacing.x * (cols - 1);
        float totalH = rect.rect.height - grid.spacing.y * (rows - 1);

        float size = Mathf.Min(totalW / cols, totalH / rows);
        grid.cellSize = new Vector2(size, size);
    }
}