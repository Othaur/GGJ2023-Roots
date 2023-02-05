using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundState
{
    Wall,
    Empty,
    Start
}

public class GenHexMaze : MonoBehaviour
{

    public List<MazeNode> GenerateMaze(HexGrid grid)
    {
        int i, j;

        int width = grid.Width;
        int height = grid.Height;

        List<MazeNode> nodes = new List<MazeNode>();

        // Make everything a wall
        for (i=0; i<width; i++)
        {
            for(j=0; j<height; j++)
            {
                MazeNode temp = new MazeNode(i, j, grid.GetObject(i, j));
                temp.State = GroundState.Wall;

                nodes.Add(temp);
            }
        }



        return nodes;
    }
}

public class MazeNode
{
    public MapGridObject Contents { get; set; }    
    public int X { get; set; }
    public int Y { get; set; }
    public GroundState State { get; set; }

    public MazeNode(int x, int y, MapGridObject item)
    {
        X = x;
        Y = y;
        Contents = item;
    }
}
