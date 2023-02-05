using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class HexGrid : MonoBehaviour
{
    private GridSystem<MapGridObject> grid;
    private MapGridObject lastGridObject; 
    [SerializeField] Transform testTransform;
    [SerializeField] Transform wallTransform;
    [SerializeField] Transform emptyTransform;
    [SerializeField] Transform startTransform;


    public int Width { get; set; }
    public int Height { get; set; }
    private void Awake()
    {
        Width = 26;
        Height = 18;
        float cellSize = 5f;

        grid = new GridSystem<MapGridObject>(Width, Height, cellSize, new Vector3((Width * cellSize)/-2f, ((Height*cellSize)*.75f/-2f)+1 ), (GridSystem<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));

    }
    private void Start()
    {
        GenHexMaze maze = new GenHexMaze();

        List<MazeNode> nodes = maze.GenerateMaze(this, new Vector2Int(13,13));

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
                            Transform tempTransform = Instantiate(wallTransform, grid.GetWorldPosition(i, j), Quaternion.identity);
                            //   tempObject.SetTransform(tempTransform);
                            grid.GetGridObject(i, j).visualTransform = tempTransform;
                            break;
                        }
                    case GroundState.Start:
                        {
                            Transform tempTransform = Instantiate(startTransform, grid.GetWorldPosition(i, j), Quaternion.identity);
                            //   tempObject.SetTransform(tempTransform);
                            grid.GetGridObject(i, j).visualTransform = tempTransform;
                            break;
                        }
                    case GroundState.Empty:
                        {
                            Transform tempTransform = Instantiate(emptyTransform, grid.GetWorldPosition(i, j), Quaternion.identity);
                            //   tempObject.SetTransform(tempTransform);
                            grid.GetGridObject(i, j).visualTransform = tempTransform;
                            break;
                        }
                }
            }
    }

    private void Update()
    {
        Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
        Debug.DrawLine(Vector3.zero, worldPos);

        if (lastGridObject != null)
        {
           // lastGridObject.Hide();
        }
        int x, y;
        // Debug.Log("Object:" + grid.GetWorldPosition(Mathf.RoundToInt(worldPos.x),Mathf.RoundToInt( worldPos.y)).ToString());
        lastGridObject = grid.GetGridObject(worldPos);
        
        
        if (lastGridObject != null)
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out x, out y);
          //  Debug.Log("Pos:" + UtilsClass.GetMouseWorldPosition() + " within " + x + "," + y);
           // lastGridObject.Show();
        }
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

}



public class MapGridObject
{
    GridSystem<MapGridObject> grid;
    int x, y;
    public Transform visualTransform;
    
    //public void Show()
    //{
    //    visualTransform.Find("Selected").gameObject.SetActive(true);
    //    visualTransform.Find("Hex").gameObject.SetActive(false);
    //    //Debug.Log("Showing cell");
    //}

    //public void Hide()
    //{
    //    visualTransform.Find("Selected").gameObject.SetActive(false);
    //    visualTransform.Find("Hex").gameObject.SetActive(true);
    //}

    public MapGridObject(GridSystem<MapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetTransform(Transform transform)
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

