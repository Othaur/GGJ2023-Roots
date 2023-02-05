using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundState
{
    Wall,
    Empty,
    Start
}

public class GenHexMaze
{

    public List<MazeNode> GenerateMaze(HexGrid grid, Vector2Int startPos)
    {
        int i, j;

        int width = grid.Width;
        int height = grid.Height;

        List<MazeNode> nodes = new List<MazeNode>();
        List<MazeNode> pathList = new List<MazeNode>();
        List<MazeNode> wallList = new List<MazeNode>();

        MazeNode startNode;
        // Make everything a wall
        for (j=0; j<height; j++)
        {
            for(i=0; i<width; i++)
            {
                MazeNode temp = new MazeNode(i, j, grid.GetObject(i, j));
                temp.State = GroundState.Wall;

                if (i == startPos.x && j == startPos.y)
                {
                    temp.State = GroundState.Start;
                    Debug.Log("Start:" + i + "," + j);
                    startNode = temp;
                }
                nodes.Add(temp);
            }
        }

        // Add start node to path list and neighbours to wall list
        List<Vector3Int> neighbours = grid.GetCellNeighbours(startPos.x, startPos.y);
        foreach (var neighbour in neighbours)
        {
            int index = neighbour.x + (neighbour.y * height);
            if (nodes[index].State == GroundState.Wall)
            {
                Debug.Log("AAAAAAHAHHH " + neighbour.x + "," + neighbour.y);
                wallList.Add(nodes[index]);
            }
        }

        while()

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
