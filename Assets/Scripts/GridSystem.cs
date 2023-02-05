using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridSystem <TGridObject>
{
    private int width;
    private int height;
    private TGridObject[,] gridArray;
    //private TextMesh[,] debugTextArray;
    private float cellSize;
    private Vector3 origin;
    public bool showDebug;

    private const float HEX_VERT_OFFSET = 0.75f;
    
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs: EventArgs
    {
        public int x;
        public int y;
    }

    public GridSystem(int width, int height, float cellSize, Vector3 origin, Func<GridSystem<TGridObject>, int ,int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new TGridObject[width, height];
        //debugTextArray = new TextMesh[width, height];

        for(int x=0; x<gridArray.GetLength(0); x++)
            for(int y=0; y<gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x,y);
            }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return
            new Vector3(x, 0) * cellSize +
            new Vector3(0, y) * cellSize * HEX_VERT_OFFSET +
           ((Mathf.Abs(y) % 2) == 1 ? new Vector3(1, 0, 0) * cellSize * 0.5f : Vector3.zero)
            + origin;
    }

     public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 temp = worldPosition - origin;
        int roughX = Mathf.RoundToInt(temp.x / cellSize);
        int roughY = Mathf.RoundToInt(temp.y / cellSize / HEX_VERT_OFFSET);

        Vector3Int roughXY = new Vector3Int(roughX, roughY);

        List<Vector3Int> neighbourList =  GetNeighbours(roughX, roughY);

        Vector3Int closestXY = roughXY;

        foreach (Vector3Int neighbour in neighbourList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbour.x, neighbour.y)) <
                    Vector3.Distance(worldPosition, GetWorldPosition(closestXY.x, closestXY.y)))
            {
                closestXY = neighbour;
            }
        }
        Debug.DrawLine(GetWorldPosition(closestXY.x, closestXY.y), UtilsClass.GetMouseWorldPosition());
        x = closestXY.x;
        y = closestXY.y;
    }

    public List<Vector3Int> GetNeighbours(int x, int y )
    {        
        Vector3Int roughXY = new Vector3Int(x, y);

        bool oddRow = y % 2 == 1;

        List<Vector3Int> neighbourList = new List<Vector3Int>
        {
            roughXY + new Vector3Int(-1,0),
            roughXY + new Vector3Int(1, 0),

            roughXY + new Vector3Int(oddRow? -1: 1, 1),
            roughXY + new Vector3Int(0, 1),

            roughXY + new Vector3Int(oddRow? -1:1, -1),
            roughXY + new Vector3Int(0, -1),
        };

        List<Vector3Int> tempList = new List<Vector3Int>();
        foreach(var n in neighbourList)
        {
            if (n.x < 0 || n.x >= width)
            {
                continue;
            }
            if (n.y < 0 || n.y >= height)
            {
                continue;
            }
            tempList.Add(n);
        }

        return tempList;
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged( int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;

        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
        Debug.Log("ITem added");
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        return default;
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;

        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

}
    
