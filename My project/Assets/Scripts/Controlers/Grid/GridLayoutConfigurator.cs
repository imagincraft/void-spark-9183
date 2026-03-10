using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IGridLayoutConfigurator
{
    void ConfigureCellSize(
        GridLayoutGroup gridLayout,
        RectTransform gridRect,
        int rows,
        int columns);
}

public class GridLayoutConfigurator : MonoBehaviour, IGridLayoutConfigurator
{
    public void ConfigureCellSize(
        GridLayoutGroup gridLayout,
        RectTransform gridRect,
        int rows,
        int columns)
    {
        if (gridLayout == null || gridRect == null) return;

        float spacingX = gridLayout.spacing.x;
        float spacingY = gridLayout.spacing.y;

        float availW = gridRect.rect.width  - spacingX * (columns - 1);
        float availH = gridRect.rect.height - spacingY * (rows - 1);

        float cellSize = Mathf.Min(availW / columns, availH / rows);

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
