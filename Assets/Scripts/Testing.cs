using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private GridSystem<MapGridObject> grid;
    private void Start()
    {
        grid = new GridSystem<MapGridObject>(20, 10, 10f, new Vector3(-120, -10), (GridSystem<MapGridObject> g, int x, int y) => new MapGridObject(g,x,y));
        grid.showDebug = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            MapGridObject temp = grid.GetGridObject(position);
            if (temp != null)
            {
                temp.AddValue(5);
            }

        }
    }
}

public class MapGridObject
{
    const int MIN = 0;
    const int MAX = 100;

    GridSystem<MapGridObject> grid;
    int x, y;
    int value;
    
    public MapGridObject(GridSystem<MapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x,y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}

