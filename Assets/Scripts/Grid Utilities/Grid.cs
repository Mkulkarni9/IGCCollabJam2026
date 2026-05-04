using System;
using TMPro;
using UnityEngine;
public class Grid<TGridObject>
{
    public event Action<int, int> OnGridValueChanged;


    int width;
    int height;
    float cellSize;
    Vector3 origin;
    TGridObject[,] gridArray;
    TextMeshPro[,] textValuesDisplayed;

    bool isDebugMode = false;

    public Grid(int width, int height, float cellSize, Vector3 origin, Func<Grid<TGridObject>, int, int, TGridObject> createTGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        this.gridArray = new TGridObject[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = createTGridObject(this, i,j);
            }
        }

        if(isDebugMode)
        {
            textValuesDisplayed = new TextMeshPro[width, height];
            
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
                    textValuesDisplayed[i, j] = CreateWorldText.CreateTextMesh(gridArray[i, j]?.ToString(), new Vector3(i + 0.5f, j + 0.5f) * cellSize + origin, cellSize);
                }
            }

            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (int x, int y) =>
            {
                textValuesDisplayed[x, y].text = gridArray[x, y].ToString();
            };
            
        }
        

    }

    public bool CheckIfValueWithinBounds(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height) return true;
        else return false;
        
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    void GetXYWorldPosition(Vector3 position, out int x, out int y)
    {
        //Vector3 worldPosition = WorldPosition.GetWorldPosition(mousePosition);
        x = Mathf.FloorToInt((position - origin).x / cellSize);
        y = Mathf.FloorToInt((position - origin).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject gridObject)
    {
        if(CheckIfValueWithinBounds(x,y))
        {
            gridArray[x, y] = gridObject;
            textValuesDisplayed[x, y].text = gridObject.ToString();

            TriggerGridValueChange(x, y);
        }
    }

    

    public void SetGridObject(Vector3 position, TGridObject gridObject)
    {
        int x, y;
        GetXYWorldPosition(position, out x, out y);
        if (CheckIfValueWithinBounds(x, y))
        {
            gridArray[x, y] = gridObject;
            textValuesDisplayed[x, y].text = gridObject.ToString();

            TriggerGridValueChange(x,y);
        }
        
    }

    public void TriggerGridValueChange(int x, int y)
    {
        OnGridValueChanged?.Invoke(x, y);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (CheckIfValueWithinBounds(x, y))
        {
            return gridArray[x, y];
        }

        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 position)
    {
        int x, y;
        GetXYWorldPosition(position, out x, out y);
        if (CheckIfValueWithinBounds(x, y))
        {
            return gridArray[x, y];
        }

        return default(TGridObject);
    }
}
