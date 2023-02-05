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
        int nodeIndex;
        int nodeCount;

        int width = grid.Width;
        int height = grid.Height;

        List<MazeNode> nodes = new List<MazeNode>();
        List<MazeNode> pathList = new List<MazeNode>();
        List<MazeNode> wallList = new List<MazeNode>();
        List<Vector3Int> neighbours = new List<Vector3Int>();

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
                    startNode = temp;
                    pathList.Add(startNode);
                }
                nodes.Add(temp);
            }
        }

        // Add start node to path list and neighbours to wall list                
        neighbours = grid.GetCellNeighbours(startPos.x, startPos.y);       
        foreach (var neighbour in neighbours)
        {
            int index = neighbour.x + (neighbour.y * width);
            if (nodes[index].State == GroundState.Wall)
            {
                wallList.Add(nodes[index]);
            }
        }

        while (wallList.Count > 0 || wallList.Count > 50)
        {
            int newIndex = Random.Range(0, wallList.Count);
            MazeNode current = wallList[newIndex];

            // Get neighbours
            neighbours.Clear();
            neighbours = grid.GetCellNeighbours(current.X, current.Y);
            // Check if only one neighbour is empty
            nodeCount = 0;
            foreach (var n in neighbours)
            {
                nodeIndex = n.x + (n.y * width);
                if (nodes[nodeIndex].State == GroundState.Empty || nodes[nodeIndex].State == GroundState.Start)
                {
                    nodeCount++;
                }
            }
            // If only one neighbout was empty add to path
            if (nodeCount == 1)
            {
                current.State = GroundState.Empty;
                pathList.Add(current);

                foreach (var n in neighbours)
                {
                    nodeIndex = n.x + (n.y * width);
                    if (nodes[nodeIndex].State == GroundState.Wall)

                        if (!wallList.Contains(nodes[nodeIndex]))
                        {
                             wallList.Add(nodes[nodeIndex]);
                        }
                }
            }
                        
            wallList.RemoveAt(newIndex);

            nodeIndex = current.X + (current.Y * width);

            nodes[nodeIndex] = current;
            pathList.Add(current);

        }

        return nodes;
    }


    void DisplayList(List<MazeNode> list)
    {
        foreach(var i in list)
        {
            Debug.Log("node: " + i.X + "," + i.Y +" -> " + i.State);
        }
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
