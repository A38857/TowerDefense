using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding
{
    private List<List<int>> grid;
    private int width, height;

    public AStarPathFinding(List<List<int>> map)
    {
        this.grid = map;
        this.height = map.Count;
        this.width = map[0].Count;
    }

    private class Node
    {
        public Vector2Int position;
        public Node parent;
        public float gCost, hCost, fCost;

        public Node(Vector2Int pos, Node parent, float gCost, float hCost)
        {
            this.position = pos;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
            this.fCost = gCost + hCost;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

        Node startNode = new Node(start, null, 0, GetDistance(start, end));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList.Sort((a, b) => a.fCost.CompareTo(b.fCost));
            Node currentNode = openList[0];

            if (currentNode.position == end)
                return RetracePath(currentNode);

            openList.Remove(currentNode);
            closedSet.Add(currentNode.position);

            // Check 4 Axis
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentNode.position + direction;
                if (!IsValid(neighborPos) || closedSet.Contains(neighborPos)) continue;

                float newGCost = currentNode.gCost + 1;
                Node neighborNode = new Node(neighborPos, currentNode, newGCost, GetDistance(neighborPos, end));

                Node existingNode = openList.Find(n => n.position == neighborPos);
                if (existingNode != null && newGCost >= existingNode.gCost) continue;

                openList.Add(neighborNode);
            }
        }

        return null; 
    }

    private bool IsValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height && grid[pos.y][pos.x] == 0;
    }

    private float GetDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> RetracePath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.position);
            node = node.parent;
        }
        path.Reverse();
        return path;
    }
}
