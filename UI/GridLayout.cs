using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayout : LayoutGroup
{
    [SerializeField]
    private int rows = 1;
    [SerializeField]
    private int columns = 1;
    [SerializeField]
    private Vector2 cellSize = new Vector2(50f, 50f);
    [SerializeField]
    private Vector2 spacing = new Vector2(5f, 5f);

    private enum Layout { Equal, Width, Height, SetColumns, SetRows }
    [SerializeField]
    private Layout layout = Layout.Equal;
    [SerializeField]
    private bool scaleToX = false;
    [SerializeField]
    private bool scaleToY = false;
    [SerializeField]
    private bool centerCells = true;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (layout == Layout.Equal || layout == Layout.Height || layout == Layout.Width)
        {
            scaleToX = true;
            scaleToY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (layout == Layout.Height || layout == Layout.SetRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        else if (layout == Layout.Width || layout == Layout.SetColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows); ;

        if (cellHeight >= cellWidth)
        {
            cellHeight = cellWidth;
        }
        else
        {
            cellWidth = cellHeight;
        }

        cellSize.x = scaleToX ? cellWidth : cellSize.x;
        cellSize.y = scaleToY ? cellHeight : cellSize.y;

        if (centerCells)
        {
            padding.top = Mathf.CeilToInt((parentHeight - (rows * (cellHeight + spacing.y))) / 2f);
            //padding.bottom = Mathf.CeilToInt((parentHeight - (rows * (cellHeight + spacing.y))) / 2f);
            padding.left = Mathf.CeilToInt((parentWidth - (columns * (cellWidth + spacing.x))) / 2f);
            //padding.right = Mathf.CeilToInt((parentWidth - (columns * (cellWidth + spacing.x))) / 2f);

            padding.top = padding.top < 0 ? 0 : padding.top;
            //padding.bottom = padding.bottom < 0 ? 0 : padding.bottom;
            padding.left = padding.left < 0 ? 0 : padding.left;
            //padding.right = padding.right < 0 ? 0 : padding.right;
        }

        int columnCount;
        int rowCount;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}
