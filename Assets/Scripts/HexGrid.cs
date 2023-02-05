using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class HexGrid : MonoBehaviour
{
    private GridSystem<MapGridObject> grid;
    private MapGridObject lastGridObject;
 // [SerializeField] Transform pfHex;
    [SerializeField] Transform WallTransform;
    [SerializeField] Transform EmnptyTransform;
    [SerializeField] Transform StartTransform;

    public int Width { get; set; }
    public int Height { get; set; }
    private void Awake()
    {
        Width = 15;
        Height = 9;
        float cellSize = 10f;

        grid = new GridSystem<MapGridObject>(Width, Height, cellSize, new Vector3((Width * cellSize)/-2f, ((Height*cellSize)*.75f/-2f)+1 ), (GridSystem<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));

        //for( int x=0; x<Width; x++)
        //    for(int y =0; y<Height; y++)
        //    {
        //       Transform visualTransform =  Instantiate(WallTransform, grid.GetWorldPosition(x, y), Quaternion.identity);
        //        grid.GetGridObject(x, y).visualTransform = visualTransform;
        //        grid.GetGridObject(x,y).Hide();
        //    }
    }
    private void Start()
    {
        GenHexMaze maze = new GenHexMaze();

        List<MazeNode> nodes = maze.GenerateMaze(this);

        int index=0; 
        for(int i =0;i<Width; i++)
            for(int j =0; j<Height; j++)
            {
                index = i + (j * Width);
                if (nodes[index].State == GroundState.Wall)
                {
                    Transform tempTransform = Instantiate(WallTransform, grid.GetWorldPosition(i, j), Quaternion.identity);
                    MapGridObject tempObject = grid.GetGridObject(i, j);
                    tempObject.SetTransform(tempTransform);
                    grid.SetGridObject(i, j, tempObject);
                }
            }

    }

    private void Update()
    {
        Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
        Debug.DrawLine(Vector3.zero, worldPos);

        if (lastGridObject != null)
        {
            lastGridObject.Hide();
        }
        int x, y;
        Debug.Log("Object:" + grid.GetWorldPosition(Mathf.RoundToInt(worldPos.x),Mathf.RoundToInt( worldPos.y)).ToString());
        lastGridObject = grid.GetGridObject(worldPos);
        
        
        if (lastGridObject != null)
        {
            Debug.Log("Pos:" + UtilsClass.GetMouseWorldPosition() + " within " + lastGridObject.visualTransform.position);
            lastGridObject.Show();
        }
    }

    public MapGridObject GetObject(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    public void SetObject(int x, int y, MapGridObject item)
    {
        grid.SetGridObject(x, y, item);
    }

}



public class MapGridObject
{
    GridSystem<MapGridObject> grid;
    int x, y;
    public Transform visualTransform;
    
    public void Show()
    {
        visualTransform.Find("Selected").gameObject.SetActive(true);
        visualTransform.Find("Hex").gameObject.SetActive(false);
        //Debug.Log("Showing cell");
    }

    public void Hide()
    {
        visualTransform.Find("Selected").gameObject.SetActive(false);
        visualTransform.Find("Hex").gameObject.SetActive(true);
    }

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

