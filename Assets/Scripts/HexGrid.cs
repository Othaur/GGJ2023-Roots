using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class HexGrid : MonoBehaviour
{
    private GridSystem<MapGridObject> grid;
    private MapGridObject lastGridObject; 
    [SerializeField] GameObject testTransform;
    [SerializeField] GameObject wallTransform;
    [SerializeField] GameObject emptyTransform;
    [SerializeField] GameObject startTransform;

    public Vector2Int currentTile;
    List<MazeNode> nodes;


    public int Width { get; set; }
    public int Height { get; set; }
    private void Awake()
    {
        Width = 30; //26
        Height = 18; //18
        float cellSize = 5f; //5

        grid = new GridSystem<MapGridObject>(Width, Height, cellSize, new Vector3((Width * cellSize)/-2f, ((Height*cellSize)*.75f/-2f)+1 ), (GridSystem<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));

    }
    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        GenHexMaze maze = new GenHexMaze();
        int startX = 13;
        int startY = 17;

        nodes = maze.GenerateMaze(this, new Vector2Int(startX, startY));

        int index = 0;
        for (int j = 0; j < Height; j++)
            for (int i = 0; i < Width; i++)
            {
                index = i + (j * Width);
                MapGridObject tempObject = grid.GetGridObject(i, j);
                MazeNode tempNode = nodes[index];

                switch (tempNode.State)
                {
                    case GroundState.Wall:
                        {
                            GameObject tempTransform = Instantiate(wallTransform, grid.GetWorldPosition(i, j)+ new Vector3(0,0,-2), Quaternion.identity);
                            grid.GetGridObject(i, j).visualTransform = tempTransform;                            
                            break;
                        }
                    case GroundState.Start:
                        {
                            GameObject tempTransform = Instantiate(startTransform, grid.GetWorldPosition(i, j) + new Vector3(0, 0, 20), Quaternion.identity);
                            grid.GetGridObject(i, j).visualTransform = tempTransform;
                            ShowTile(tempTransform.gameObject);
                            break;
                        }
                    case GroundState.Empty:
                        {
                            GameObject tempTransform = Instantiate(emptyTransform, grid.GetWorldPosition(i, j) + new Vector3(0, 0, 10), Quaternion.identity);
                            grid.GetGridObject(i, j).SetTransform( tempTransform);
                            break;
                        }
                }
            }

        // Make neighbours visible
        List<Vector3Int> neighbours = grid.GetNeighbours(startX, startY);

        foreach (var n in neighbours)
        {
            MapGridObject temp = grid.GetGridObject(n.x, n.y);
            ShowTile(temp.visualTransform.gameObject);
        }
    }

    private void Update()
    {
        Vector3 worldPos = UtilsClass.GetMouseWorldPosition();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            
                int i, j;
                grid.GetXY(UtilsClass.GetMouseWorldPosition(), out i, out j);

                int index = i + (j * Width);
            if (CanGrow(worldPos))
            {
                
                    List<Vector3Int> neighbours = grid.GetNeighbours(i, j);

                    foreach (var n in neighbours)
                    {
                        MapGridObject temp = grid.GetGridObject(n.x, n.y);
                        ShowTile(temp.visualTransform.gameObject);
                    }
                
            }
            //for (int i = 0; i < Width; i++)
            //    for (int j = 0; j < Height; j++)
            //    {
            //        var temp = grid.GetGridObject(i, j);
            //        Destroy(temp.visualTransform.gameObject);
            //        temp.ClearTransform();
            //    }

            //GenerateMaze();
        }

        if (lastGridObject != null)
        {
           // lastGridObject.Hide();
        }
        int x, y;
        lastGridObject = grid.GetGridObject(worldPos);
        
        
        if (lastGridObject != null)
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);          
        }
    }

    public bool CanGrow(Vector3 mousePosition)
    {
        int i, j;
        grid.GetXY(UtilsClass.GetMouseWorldPosition(), out i, out j);
        MapGridObject currentTile = grid.GetGridObject(i, j);

        // Is the tile visible
        if (IsTileVisible(currentTile.visualTransform.gameObject))
        {
            
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out i, out j);

            int index = i + (j * Width);
            // Is the tile empty
            if (nodes[index].State == GroundState.Empty)
            {
                return true;
            }
        }

        return false;
    }
    public List<Vector3Int> GetCellNeighbours(int x, int y)
    {
        return grid.GetNeighbours(x, y);
    }

    public MapGridObject GetObject(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public void SetObject(int x, int y, MapGridObject item)
    {
        grid.SetGridObject(x, y, item);
    }

    public Vector3 GetCellCenter(Vector3 position)
    {
        int x, y;
        grid.GetXY(position, out x, out y);
        return grid.GetWorldPosition(x, y);
    }

    public bool IsTileVisible( GameObject tile)
    {
        DisplayTile display = tile.GetComponent<DisplayTile>();

        return display.IsVisible();
    }
    public void ShowTile(GameObject tile)
    {
        if (tile.GetComponent<DisplayTile>())
        {
            tile.GetComponent<DisplayTile>().Show(true);
        }
    }
}



public class MapGridObject
{
    GridSystem<MapGridObject> grid;
    int x, y;
    public GameObject visualTransform;
    
    public MapGridObject(GridSystem<MapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetTransform(GameObject transform)
    {
        this.visualTransform = transform;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearTransform()
    {
        visualTransform = null;
    }

    public bool CanBuild()
    {
        return visualTransform == null;
    }

}

