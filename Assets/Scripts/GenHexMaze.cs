using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenHexMaze : MonoBehaviour
{

    public HexGrid GenerateMaze(HexGrid grid)
    {
        int width = grid.Width;
        int height = grid.Height;

        int i, j;

        List<MazeNode> nodes = new List<MazeNode>();
        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        for(i=0; i<width; i++)
            for(j=0;j<height; j++)
            {
                MazeNode node = new MazeNode(i, j, grid.GetObject(i, j));
                nodes.Add(node);
            }

        currentPath.Add(nodes[Random.Range(0,nodes.Count)]);
        currentPath[0].State = 1;

        while (completedNodes.Count < nodes.Count)
        {
            List<int> possibleNextNodes = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / width;
            int currentNodeY = currentNodeIndex % height;

            if (currentNodeX < width - 1)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + height]) && !currentPath.Contains(nodes[currentNodeIndex + height]))
                    possibleNextNodes.Add(currentNodeIndex + height);
            }
            if (currentNodeX > 0)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - height]) && !currentPath.Contains(nodes[currentNodeIndex - height]))
                    possibleNextNodes.Add(currentNodeIndex - height);
            }

            if (currentNodeY < height - 1)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) && !currentPath.Contains(nodes[currentNodeIndex + 1]))
                    possibleNextNodes.Add(currentNodeIndex + 1);
            }
            if (currentNodeY > 0)
            {
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) && !currentPath.Contains(nodes[currentNodeIndex - 1]))
                    possibleNextNodes.Add(currentNodeIndex + height);
            }

            if (possibleNextNodes.Count > 0)
            {
                MazeNode chozenNode = nodes[possibleNextNodes[Random.Range(0, possibleNextNodes.Count)]];
                currentPath.Add(chozenNode);
                chozenNode.State = 1;
            }
            else
            {
                completedNodes.Add(currentPath[currentNodeIndex]);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

        }

        return grid;
    }
}

public class MazeNode
{
    public MapGridObject Contents { get; set; }    
    public int X { get; set; }
    public int Y { get; set; }
    public int State { get; set; }

    public MazeNode(int x, int y, MapGridObject item)
    {
        X = x;
        Y = y;
        Contents = item;
    }
}
