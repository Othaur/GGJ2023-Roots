using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private GridSystem<MapGridObject> grid;
  //  private MapGridObject lastGridObject;
 // [SerializeField] Transform pfHex;
    [SerializeField] Transform testTransform;

    public class MapGridObject
    {
        GridSystem<MapGridObject> grid;
        int x, y;
        Transform transform;

        public MapGridObject(GridSystem<MapGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTransform(Transform transform)
        {
            this.transform = transform;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearTransform()
        {
            transform = null;
        }

        public bool CanBuild()
        {
            return transform == null;
        }

    }

    //private class TestingObject
    //{
    //    public Transform visualTransform;

    //    public void Show()
    //    {
    //        visualTransform.Find("Selected").gameObject.SetActive(true);
    //    }

    //    public void Hide()
    //    {
    //        visualTransform.Find("Selected").gameObject.SetActive(false);
    //    }
    //}

    private void Awake()
    {
        int width = 10;
        int height = 6;
        float cellSize = 11f;

        grid = new GridSystem<MapGridObject>(width, height, cellSize, Vector3.zero, (GridSystem<MapGridObject> g, int x, int y) => new MapGridObject(g, x, y));

        for( int x=0; x<width; x++)
            for(int y =0; y<height; y++)
            {
                //Transform visualTransform = Instantiate(Test, grid.GetWorldPosition(x, y), Quaternion.identity);
              //  grid.GetGridObject(x, y).visualTransform = visualTransform;
             //   grid.GetGridObject(x.y).Hide();
            }
    }
    private void Start()
    {
        
        grid.showDebug = true;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);

            MapGridObject gridObject = grid.GetGridObject(x,y);
            if (gridObject.CanBuild())
            {
                Transform buildTransform = Instantiate(testTransform, grid.GetWorldPosition(x, y), Quaternion.identity);
                gridObject.SetTransform(buildTransform);
            }
            
        }
        //if (lastGridObject != null)
        //{
        //    lastGridObject.Hide();
        //}

        //lastGridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());

        //lastGridObject.Show();
    }
}



