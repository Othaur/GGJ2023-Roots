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
    private TextMesh[,] debugTextArray;
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
        debugTextArray = new TextMesh[width, height];

        for(int x=0; x<gridArray.GetLength(0); x++)
            for(int y=0; y<gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x,y);
            }

       //// if (showDebug)
       // {
       //     for (int x = 0; x < gridArray.GetLength(0); x++)
       //         for (int y = 0; y < gridArray.GetLength(1); y++)
       //         {
       //             debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
       //             Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
       //             Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
       //         }

            //OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            //{
            //    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            //};
       // }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return 
            new Vector3(x, 0) * cellSize +
            new Vector3(0,y) * cellSize * HEX_VERT_OFFSET +
           ( (y%2)==1 ? new Vector3(1,0,0) * cellSize *0.5f: Vector3.zero)
            + origin;
    }

     public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 temp = worldPosition - origin;
        x = Mathf.FloorToInt(temp.x / cellSize);
        y = Mathf.FloorToInt(temp.y / cellSize);
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
    
